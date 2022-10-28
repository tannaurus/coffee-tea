using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mess : MonoBehaviour
{
    public GameObject mess;

    private Renderer messRenderer;
    private Color dirtyMessColor;
    private Color cleanedMessColor;

    private float messiness = 1f;

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
        if (messiness != 0f) {
            messiness -= 1f / ragStrength * Time.deltaTime;
            messiness = Mathf.Clamp(messiness, 0, 1);
            mess.transform.localScale = new Vector3(messiness, mess.transform.localScale.y, messiness);
            messRenderer.material.color = Color.Lerp(cleanedMessColor, dirtyMessColor, messiness);
            Debug.Log("Cleaning mess...");
        }
    }
}
