using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) {
        Destroy(gameObject);
        Rigidbody rb = collider.attachedRigidbody;
        // spin on glass break
        rb.AddTorque(Vector3.right * 8);
    }
}
