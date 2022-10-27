using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rag : MonoBehaviour
{
    public GameObject openedRag;
    public GameObject foldedRag;
    public Color dirtyRagColor;

    public float ragStrength = 1f;

    public float dirtiness = 0;
    public bool folded = true;

    private Color cleanRagColor;
    private Renderer openedRagRenderer;
    private Renderer foldedRagRenderer;

    private Mess mess;

    // Start is called before the first frame update
    void Start()
    {
        openedRagRenderer = openedRag.GetComponent<Renderer>();
        foldedRagRenderer = foldedRag.GetComponent<Renderer>();

        cleanRagColor = openedRagRenderer.material.color;

        FoldRag();
    }

    void OnTriggerEnter(Collider collider) {
         if(collider.tag == "Mess") {
            mess = collider.gameObject.GetComponent<Mess>();
         }
    }

    void OnTriggerStay(Collider collider) {
        if(collider.tag == "Mess" && mess && dirtiness < 1f) {
            dirtiness += 1f / ragStrength * Time.deltaTime;
            ChangeRagColor();
            mess.CleanMess(ragStrength);
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Mess") {
            mess = null;
        }
    }

    public void ToggleRag() {
        if (folded) {
            OpenRag();
        } else {
            FoldRag();
        }
    }

    private void OpenRag() {
        folded = false;
        foldedRag.SetActive(false);
        openedRag.SetActive(true);
    }

    private void FoldRag() {
        folded = true;
        foldedRag.SetActive(true);
        openedRag.SetActive(false);
    }

    private void ChangeRagColor() {
        Color newColor = Color.Lerp(cleanRagColor, dirtyRagColor, dirtiness);
        openedRagRenderer.material.color = newColor;
        foldedRagRenderer.material.color = newColor;
    }
}
