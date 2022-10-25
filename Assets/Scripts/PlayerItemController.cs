using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets {
    public class PlayerItemController : MonoBehaviour
    {
		public Camera playerCamera;

        private GameObject hand;
        private Rigidbody handRb;

        // Controllers to prevent behavior from occuring while other behaviors are occuring
        enum AnimationState {dropping, grabbing, nothing}
        private AnimationState animationState = AnimationState.nothing;

        private int animationFrameCount = 45;
        private int elapsedAnimationFrames = 0;

        private Vector3 itemPlacementPosition;

        // Arbitrary placement for item when "in hand"
        private Vector3 handPosition = new Vector3(0.457f, 1.065f, 0.938f);

        private StarterAssetsInputs inputs;
        // Start is called before the first frame update
        void Start()
        {
            inputs = GetComponent<StarterAssetsInputs>();
        }

        void FixedUpdate() {
            Interact();

            if (animationState == AnimationState.dropping) {
                MoveItemToDropPosition();
            } else if (animationState == AnimationState.grabbing) {
                MoveItemToHand();
            }
        }

        private void Interact() {
            if (inputs.pickup) {
                inputs.pickup = false;

                if (animationState == AnimationState.nothing) {
                    if (hand) {
                        Debug.Log("Dropping item...");
                        DropItemInHand();
                    } else {
                        Debug.Log("Grabbing item...");
                        GrabItem();
                    }
                }
            }
        }

        private void DropItemInHand() {
            Vector3? dropLocation = GetDropLocation();
            if (dropLocation != null) {
                Debug.Log("Setting drop position...");
                itemPlacementPosition = (Vector3)dropLocation;
                animationState = AnimationState.dropping;
            } else {
                Debug.Log("No drop position found...");
                DropItemOnGround();
            }
        }

        // If the player is looking at something, return the point in space where they are looking
        private Vector3? GetDropLocation() {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 15f)) {
                    float placementOffset = hand.GetComponent<Item>().GetPlacementOffset();
                    Debug.Log(placementOffset);
                    return new Vector3(hit.point.x, hit.point.y + placementOffset, hit.point.z);
            }
            return null;
        }

        private void MoveItemToDropPosition() {
            if (hand.transform.position == itemPlacementPosition) {
                animationState = AnimationState.nothing;
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
        }

        private void MoveItemToHand() {
            if (hand.transform.localPosition == handPosition) {
                animationState = AnimationState.nothing;
                elapsedAnimationFrames = 0;
            } else {
                float interpolationRatio = (float)elapsedAnimationFrames / animationFrameCount;

                Vector3 interpolatedPosition = Vector3.Lerp(hand.transform.localPosition, handPosition, interpolationRatio);
                hand.transform.localPosition = interpolatedPosition;

                elapsedAnimationFrames = (elapsedAnimationFrames + 1) % (animationFrameCount + 1);
            }
        }

        private void GrabItem() {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 15f)) {
                if (hit.transform.gameObject.tag == "Item") {
                    hand = hit.transform.gameObject;
                    handRb = hand.GetComponent<Rigidbody>();
                    handRb.isKinematic = true;
                    hand.transform.SetParent(transform, true);
                    animationState = AnimationState.grabbing;
                }
            }
        }
    }
}