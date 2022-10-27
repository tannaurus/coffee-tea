using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mess : MonoBehaviour
{
    public GameObject mess;

    private Renderer messRenderer;
    private Color dirtyMessColor;
    private Color cleanedMessColor;

    // Start is called before the first frame update
    void Start()
    {
        messRenderer = mess.GetComponent<Renderer>();

        dirtyMessColor = messRenderer.material.color;
        cleanedMessColor = dirtyMessColor;
        // make existing color be transparent so we can lerp between dirty and clean for any mess color
        cleanedMessColor.a = 0f;
    }

    public void CleanMess(float ragStrength) {
        messRenderer.material.color = Color.Lerp(dirtyMessColor, cleanedMessColor, 1f / ragStrength * Time.deltaTime);
    }
}
