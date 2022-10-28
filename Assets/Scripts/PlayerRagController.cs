using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagController : MonoBehaviour
{
    public Rag rag;

    private PlayerItemController itemController;

    private Vector3? onRagUsePlayerPosition;
    private Vector3? ragUsePosition;

    // Start is called before the first frame update
    void Start()
    {
        itemController = GetComponent<PlayerItemController>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (itemController.inputs.interact && HoldingRag()) {
            itemController.inputs.interact = false;
            ToggleRagFold();
        }

        if (itemController.inputs.use && HoldingRag()) {
            // itemController.inputs.use = false;

            Debug.Log("Using rag...");
        }
    }

    private bool HoldingRag() {
        return itemController.HoldingItem() && itemController.item.type == ItemType.Rag;
    }

    private void ToggleRagFold() {
        if (!rag) {
            rag = itemController.item.GetComponent<Rag>();
        }

        rag.ToggleRag();
    }

    private void StartUsingRag() {
        onRagUsePlayerPosition = transform.position;
        ragUsePosition = GetRagUseLocation();

    }

    private void StopUsingRag() {
        onRagUsePlayerPosition = null;
        ragUsePosition = null;
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
