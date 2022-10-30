using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtinguisherController : MonoBehaviour
{

    public Extinguisher extinguisher;

    private PlayerItemController itemController;

    void Start()
    {
        itemController = GetComponent<PlayerItemController>();
    }

    void FixedUpdate() {
        if (itemController.inputs.use && HoldingExtinguisher()) {
            itemController.inputs.use = false;
            Debug.Log("Use extinguisher...");
        }
    }

    bool HoldingExtinguisher() {
        return itemController.HoldingItem() && itemController.item.type == ItemType.Extinguisher;
    }
}
