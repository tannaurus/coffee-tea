using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    public GameObject smoke;
    public GameObject brokenLaptop;

    public GameObject workingScreen;
    public GameObject brokenScreen;
    public GameObject deadScreen;

    public float health = 100f;
    public float fireHealth = 100f;

    private bool wet = false; // got wet
    private bool onFire = false; // actively on fire
    private bool dead = false; // took too much damange

    void Start()
    {
        smoke.SetActive(false);
        
        brokenScreen.SetActive(false);
        deadScreen.SetActive(false);
        workingScreen.SetActive(true);
    }

    void Update() {
        if (dead) {
            return;
        }

        if (onFire && fireHealth <= 0f) {
            PutOutFire();
        }

        if (onFire && health > 0f) {
            TakeDamange();
        }

        if (health <= 0f) {
            KillLaptop();
        }
    }

    void OnTriggerEnter(Collider collider){
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
   
    void OnParticleCollision(GameObject particle) {
        if (fireHealth > 0) {
            ApplyExtinguisherToFire();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (!dead && collision.relativeVelocity.magnitude > 2) {
            KillLaptop();
        }
    }

    public void GetWet() {
        if (wet) {
            return;
        }

        wet = true;
        onFire = true;
        fireHealth = 100f; // reset fire health
        smoke.SetActive(true);

        workingScreen.SetActive(false);
        deadScreen.SetActive(false);
        brokenScreen.SetActive(true);
    }

    private void TakeDamange() {
        health -= 5f * Time.deltaTime;
    }

    private void ApplyExtinguisherToFire() {
        fireHealth -= 5f;
    }

    private void PutOutFire() {
        onFire = false;
        smoke.SetActive(false);
    }

    private void KillLaptop() {
        smoke.SetActive(false);
        dead = true;
        onFire = false;
        workingScreen.SetActive(false);
        brokenScreen.SetActive(false);
        deadScreen.SetActive(true);
        Destroy(gameObject);
        brokenLaptop = Instantiate(brokenLaptop);
        brokenLaptop.transform.position = gameObject.transform.position;
    }
}
