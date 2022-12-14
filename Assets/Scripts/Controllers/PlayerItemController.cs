using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerItemController : MonoBehaviour
{
    public float playerReach = 8f;
    public float playerThrowForce = 400f;


    public Camera playerCamera;
    public GameObject cameraRoot;

    public Item item;
    public GameObject hand;
    public Rigidbody handRb;
    public StarterAssets.StarterAssetsInputs inputs;

    private PlayerRagController ragController;
    private PlayerExtinguisherController extinguisherController;

    // Controllers to prevent behavior from occuring while other behaviors are occuring
    public enum AnimationState {Dropping, Grabbing, Nothing}
    public AnimationState animationState = AnimationState.Nothing;

    private int animationFrameCount = 45;
    private int elapsedAnimationFrames = 0;

    private Vector3 itemPlacementPosition;

    // Arbitrary placement for item when "in hand"
    private Vector3 handPositionSteady = new Vector3(0.457f, 1.065f, 0.938f);
    private Vector3 handRotationSteady = new Vector3(0f, 0f, 0f);


    private Vector3 handPositionWithVision = new Vector3(0.470999986f,-0.200000003f, 1.005f);
    private Vector3 handRotationWithVision = new Vector3(359.947998f, 345f, 359.519989f);

    void Start()
    {
        inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
        ragController = GetComponent<PlayerRagController>();
        extinguisherController = GetComponent<PlayerExtinguisherController>();
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

    public bool ItemInHoldingPosition() {
        if (!HoldingItem()) {
            return false;
        }

        if (item.holdPosition == ItemHoldPosition.Steady) {
            return hand.transform.position == handPositionSteady;
        } 

        if (item.holdPosition == ItemHoldPosition.WithVision) {
            return hand.transform.position == handPositionWithVision;
        }

        return false;
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
        if (item.type == ItemType.Extinguisher) {
            extinguisherController.extinguisher = null;
        }

        item = null;
    }

    private void MoveItemToHand() {
        if (elapsedAnimationFrames == animationFrameCount) {
            animationState = AnimationState.Nothing;
            elapsedAnimationFrames = 0;
        } else {
            float interpolationRatio = (float)elapsedAnimationFrames / animationFrameCount;

            if (item.holdPosition == ItemHoldPosition.Steady) {
                hand.transform.localPosition = Vector3.Lerp(hand.transform.localPosition, handPositionSteady, interpolationRatio);
                // it'd be nice to rotate this gradually
                hand.transform.localEulerAngles = handRotationSteady;
            }

            if (item.holdPosition == ItemHoldPosition.WithVision) {
                hand.transform.localPosition = Vector3.Lerp(hand.transform.localPosition, handPositionWithVision, interpolationRatio);
                // it'd be nice to rotate this gradually
                hand.transform.localEulerAngles = handRotationWithVision;
            }

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
                if (item.holdPosition == ItemHoldPosition.Steady) {
                    hand.transform.SetParent(transform, true);
                }
                if (item.holdPosition == ItemHoldPosition.WithVision) {
                    hand.transform.SetParent(cameraRoot.transform, true);
                }

                animationState = AnimationState.Grabbing;
            }
        }
    }
}