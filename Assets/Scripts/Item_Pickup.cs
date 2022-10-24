using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Pickup : MonoBehaviour
{
    public Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello world");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log("Hit!", hit.transform.gameObject);
        }
    }
}

    // // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    // void FixedUpdate()
    // {
    //     // Bit shift the index of the layer (8) to get a bit mask
    //     int layerMask = 1 << 8;

    //     // This would cast rays only against colliders in layer 8.
    //     // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
    //     layerMask = ~layerMask;

    //     RaycastHit hit;
    //     // Does the ray intersect any objects excluding the player layer
    //     if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
    //     {
    //         Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
    //         Debug.Log("Did Hit");
    //     }
    //     else
    //     {
    //         Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
    //         Debug.Log("Did not Hit");
    //     }
    // }