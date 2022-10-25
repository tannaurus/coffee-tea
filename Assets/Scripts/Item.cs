using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Some items have complex structures where one collider doesn't make sense
    // This class should be assigned to each of those GameObjects and reference back to a single parent
    // that will moved when an item is picked up by the player.
    public GameObject parent;
}
