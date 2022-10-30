using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    public GameObject smoke;

    private bool damaged = false;

    // Start is called before the first frame update
    void Start()
    {
        smoke.SetActive(false);
    }

    public void Smoke() {
        if (damaged) {
            return;
        }

        smoke.SetActive(true);
        damaged = true;
    }


    void OnTriggerEnter(Collider collider){
        Debug.Log(collider.tag);
        if (collider.gameObject.tag == "Item") {
            Item item = collider.gameObject.GetComponent<Item>();
            if (item.type == ItemType.Drink) {
                Drink drink = collider.gameObject.GetComponent<Drink>();
                drink.laptop = gameObject.GetComponent<Laptop>();
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Item") {
            Item item = collider.gameObject.GetComponent<Item>();
            if (item.type == ItemType.Drink) {
                Drink drink = collider.gameObject.GetComponent<Drink>();
                drink.laptop = gameObject.GetComponent<Laptop>();
            }
        }
    }
}
