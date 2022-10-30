using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Standard, Drink, Rag, Extinguisher}
public enum ItemHoldPosition {Steady, WithVision}

public class Item : MonoBehaviour
{
    // Reference to the child of the Item that is considered the main GameObject/Mesh of the item rendered for the player
    // Mostly used to calculate the height of the mesh so the placement offset is correct
    public GameObject mainItem;

    public ItemType type = ItemType.Standard; 

    public ItemHoldPosition holdPosition = ItemHoldPosition.Steady;

    // calculated by the PlayerItemController when first picked up
    // and set here for future reference
    public float placementOffset;

    public float GetPlacementOffset() {
        if (placementOffset != 0) {
            return placementOffset;
        }

        Vector3 maxVertLocations = new Vector3(0, 0, 0);
        Vector3 minVertLocations = new Vector3(0, 0, 0);

        Vector3[] verts = mainItem.GetComponent<MeshCollider>().sharedMesh.vertices;
        foreach (Vector3 vert in verts) {
            if (vert.x > maxVertLocations.x) {
                maxVertLocations.Set(vert.x, maxVertLocations.y, maxVertLocations.z);
            }
            if (vert.y > maxVertLocations.y) {
                maxVertLocations.Set(maxVertLocations.x, vert.y, maxVertLocations.z);
            }
            if (vert.z > maxVertLocations.z) {
                maxVertLocations.Set(maxVertLocations.x, maxVertLocations.y, vert.z);
            }

            if (vert.x < minVertLocations.x) {
                minVertLocations.Set(vert.x, minVertLocations.y, minVertLocations.z);
            }
            if (vert.y < minVertLocations.y) {
                minVertLocations.Set(minVertLocations.x, vert.y, minVertLocations.z);
            }
            if (vert.z < minVertLocations.z) {
                minVertLocations.Set(minVertLocations.x, minVertLocations.y, vert.z);
            }
        }



        return (maxVertLocations.y - minVertLocations.y) / 2;
    }
}
