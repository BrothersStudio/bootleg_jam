using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDelay : MonoBehaviour
{
    public void Display()
    {
        Invoke("StopParticles", 0.1f);

        Destroy(this.gameObject, 1f);
    }

    void StopParticles()
    {
        var emission = GetComponent<ParticleSystem>().emission;
        emission.enabled = false;
    }
}