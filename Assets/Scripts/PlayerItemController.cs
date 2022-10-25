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
            }
        }

        private void Interact() {
            if (inputs.pickup) {
                inputs.pickup = false;

                Debug.Log("Interacting");
                if (animationState == AnimationState.nothing) {
                    if (hand) {
                        DropItemInHand();
                    } else {
                        GrabItem();
                    }
                }
            }
        }

        private void DropItemInHand() {
            Vector3? dropLocation = GetDropLocation();
            if (dropLocation != null) {
                itemPlacementPosition = (Vector3)dropLocation;
            } else {
                DropItemOnGround();
            }
            
            animationState = AnimationState.dropping;
        }

        // If the player is looking at something, return the point in space where they are looking
        private Vector3? GetDropLocation() {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 10f)) {
                    return hit.point;
            }
            return null;
        }

        private void MoveItemToDropPosition() {
            if (hand.transform.position == itemPlacementPosition) {
                animationState = AnimationState.nothing;
                
                ReleaseItem();
            } else {
                float interpolationRatio = (float)elapsedAnimationFrames / animationFrameCount;

                Vector3 interpolatedPosition = Vector3.Lerp(hand.transform.position, itemPlacementPosition, interpolationRatio);

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

        private void GrabItem() {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 10f)) {
                if (hit.transform.gameObject.tag == "Item") {
                    hand = hit.transform.gameObject;
                    handRb = hand.GetComponent<Rigidbody>();
                    handRb.isKinematic = true;
                    hand.transform.SetParent(transform, true);
                    hand.transform.localPosition = handPosition;
                }
            }
        }
    }
}