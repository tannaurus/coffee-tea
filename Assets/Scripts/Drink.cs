using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{
    // shoud reference item's drink mesh. used to disable on impact
    public GameObject drink;
    // should reference spill object created on impact
    public GameObject spill;

    // when a laptop detects a drink near itself, it will set this value and remove it when the drink leaves it's trigger space
    public Laptop laptop;

    private bool spilt = false;


    void Update() {
        if (spilt && laptop) {
            laptop.GetWet();
        }   
    }

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
