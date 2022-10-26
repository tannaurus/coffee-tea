using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagController : MonoBehaviour
{
    private PlayerItemController itemController;
    private Rag rag;

    // Start is called before the first frame update
    void Start()
    {
        itemController = GetComponent<PlayerItemController>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (itemController.inputs.useItem) {
            itemController.inputs.useItem = false;
            UseRag();
        }
    }

    private void UseRag() {
        if (itemController.HoldingItem() && itemController.item.type == ItemType.Rag) {
            if (!rag) {
                rag = itemController.item.GetComponent<Rag>();
            }

            rag.ToggleRag();
        }
    }
}
