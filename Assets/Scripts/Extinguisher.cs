using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{

    public ParticleSystem particles;

    void Start() {
        particles.Stop(false);
    }

    public void ToggleExtinguisher() {
        if (particles.isEmitting) {
            particles.Stop();
        } else {
            particles.Play();
        }
    }
}
