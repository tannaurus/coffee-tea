using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    public bool damaged = false;

    public GameObject smoke;

    // Start is called before the first frame update
    void Start()
    {
        smoke.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged && !smoke.activeSelf) {
            smoke.SetActive(true);
        }
    }
}
