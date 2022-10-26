using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rag : MonoBehaviour
{
    public GameObject openedRag;
    public GameObject foldedRag;

    public int dirtiness = 0;
    public bool folded = true;

    // Start is called before the first frame update
    void Start()
    {
        FoldRag();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
