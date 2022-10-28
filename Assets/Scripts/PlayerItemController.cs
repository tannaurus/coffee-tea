using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerItemController : MonoBehaviour
{
    public float playerReach = 8f;
    public float playerThrowForce = 400f;


    public Camera playerCamera;

    public Item item;
    public GameObject hand;
    public Rigidbody handRb;
    public StarterAssets.StarterAssetsInputs inputs;

    private PlayerRagController ragController;

    // Controllers to prevent behavior from occuring while other behaviors are occuring
    enum AnimationState {Dropping, Grabbing, Nothing}
    private AnimationState animationState = AnimationState.Nothing;

    private int animationFrameCount = 45;
    private int elapsedAnimationFrames = 0;

    private Vector3 itemPlacementPosition;

    // Arbitrary placement for item when "in hand"
    private Vector3 handPosition = new Vector3(0.457f, 1.065f, 0.938f);
    private Vector3 handRotation = new Vector3(0f, 0f, 0f);

    void Start()
    {
        inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
        ragController = GetComponent<PlayerRagController>();
    }

    void FixedUpdate() {
        Interact();
        Throw();

        if (animationState == AnimationState.Dropping) {
            MoveItemToDropPosition();
        } else if (animationState == AnimationState.Grabbing) {
            MoveItemToHand();
        }
    }

    public bool HoldingItem() {
        return animationState == AnimationState.Nothing && hand && item;
    }

    private void Interact() {
        if (inputs.pickupItem) {
            inputs.pickupItem = false;
            if (animationState == AnimationState.Nothing) {
                if (hand) {
                    DropItemInHand();
                } else {
                    GrabItem();
                }
            }
        }
    }

    private void Throw() {
        if (inputs.throwItem) {
            inputs.throwItem = false;
            if (HoldingItem()) {
                handRb.isKinematic = false;
                handRb.AddForce(playerCamera.transform.forward * playerThrowForce);
                ReleaseItem();
            }
        }
    }

    private void DropItemInHand() {
        Vector3? dropLocation = GetDropLocation();
        if (dropLocation != null) {
            itemPlacementPosition = (Vector3)dropLocation;
            animationState = AnimationState.Dropping;
        } else {
            DropItemOnGround();
        }
    }

    // If the player is looking at something, return the point in space where they are looking
    private Vector3? GetDropLocation() {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerReach)) {
                float placementOffset = hand.GetComponent<Item>().GetPlacementOffset();
                return new Vector3(hit.point.x, hit.point.y + placementOffset, hit.point.z);
        }
        return null;
    }

    private void MoveItemToDropPosition() {
        if (hand.transform.position == itemPlacementPosition) {
            animationState = AnimationState.Nothing;
            itemPlacementPosition = Vector3.zero;
            elapsedAnimationFrames = 0;
            
            ReleaseItem();
        } else {
            float interpolationRatio = (float)elapsedAnimationFrames / animationFrameCount;

            Vector3 interpolatedPosition = Vector3.Lerp(hand.transform.position, itemPlacementPosition, interpolationRatio);
            hand.transform.position = interpolatedPosition;

            elapsedAnimationFrames = (elapsedAnimationFrames + 1) % (animationFrameCount + 1);
        }
    }

    private void DropItemOnGround() {
        ReleaseItem();
    }

    private void ReleaseItem() {
        hand.transform.SetParent(null);
        hand = null;
        handRb.isKinematic = false;
        handRb = null;

        if (item.type == ItemType.Rag) {
            ragController.rag = null;
        }

        item = null;
    }

    private void MoveItemToHand() {
        if (elapsedAnimationFrames == animationFrameCount) {
            animationState = AnimationState.Nothing;
            elapsedAnimationFrames = 0;
        } else {
            float interpolationRatio = (float)elapsedAnimationFrames / animationFrameCount;

            hand.transform.localPosition = Vector3.Lerp(hand.transform.localPosition, handPosition, interpolationRatio);

            // it'd be nice to rotate this gradually
            hand.transform.localEulerAngles = handRotation;

            elapsedAnimationFrames = (elapsedAnimationFrames + 1) % (animationFrameCount + 1);
        }
    }

    private void GrabItem() {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerReach)) {
            if (hit.transform.gameObject.tag == "Item") {
                hand = hit.transform.gameObject;
                item = hand.GetComponent<Item>();
                handRb = hand.GetComponent<Rigidbody>();

                handRb.isKinematic = true;
                hand.transform.SetParent(transform, true);

                animationState = AnimationState.Grabbing;
            }
        }
    }
}