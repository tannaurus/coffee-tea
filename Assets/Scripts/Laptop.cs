using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    public GameObject smoke;

    private float health = 100f;

    private bool damaged = false;

    // Start is called before the first frame update
    void Start()
    {
        smoke.SetActive(false);
    }

    void Update() {
        if (damaged && health > 0f) {
            TakeDamange();
        }

        if (health <= 0f) {
            Break();
        }
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

    public void Smoke() {
        if (damaged) {
            return;
        }

        smoke.SetActive(true);
        damaged = true;
    }

    private void TakeDamange() {
        health -= 5f * Time.deltaTime;
    }

    private void Break() {
        smoke.SetActive(false);
    }


}
