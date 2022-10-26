using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    // shoud reference item's drink mesh. used to disable on impact
    public GameObject drink;
    // should reference spill object created on impact
    public GameObject spill;

    private bool spilt = false;
    

    void OnCollisionEnter(Collision collision) {
        if (!spilt && collision.relativeVelocity.magnitude > 2) {
            BoxCollider collidedCollider = collision.gameObject.GetComponent<BoxCollider>();
            Vector3 hitPoint = collidedCollider.ClosestPointOnBounds(transform.position);
            Instantiate(spill, hitPoint, Quaternion.identity);
            spilt = true;
            Destroy(drink);
        }
    }


}
