using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagController : MonoBehaviour
{
    public Rag rag;

    private PlayerItemController itemController;


    // Start is called before the first frame update
    void Start()
    {
        itemController = GetComponent<PlayerItemController>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HoldingRag() && !rag) {
            rag = itemController.item.GetComponent<Rag>();
        }

        if (itemController.inputs.interact && HoldingRag()) {
            itemController.inputs.interact = false;
            ToggleRagFold();
        }

        if (itemController.inputs.use && HoldingRag()) {
            // itemController.inputs.use = false;

            Debug.Log("Using rag...");
        }
    }

    public void PlayerMoved(Vector2 location) {
        if (!itemController.ItemInHoldingPosition()) {
            return;
        }

        Debug.Log("Player moved...");
        itemController.animationState = PlayerItemController.AnimationState.Grabbing;
    }

    private bool HoldingRag() {
        return itemController.HoldingItem() && itemController.item.type == ItemType.Rag;
    }

    private void ToggleRagFold() {
        rag.ToggleRag();
    }

    private Vector3? GetRagUseLocation() {
        RaycastHit hit;
        if (Physics.Raycast(itemController.playerCamera.transform.position, itemController.playerCamera.transform.forward, out hit, itemController.playerReach)) {
            if (hit.transform.gameObject.tag == "Mess") {
                return hit.transform.position;
            }
        }
        return null;
    }
}
