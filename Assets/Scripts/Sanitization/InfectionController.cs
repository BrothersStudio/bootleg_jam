using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionController : MonoBehaviour
{
    ParticleSystem particles;

    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>(true);

        ParticleSystem.MainModule settings = particles.main;
        switch (gameObject.tag)
        {
            case "Red":
                settings.startColor = new ParticleSystem.MinMaxGradient(Color.red);
                break;
            case "Green":
                settings.startColor = new ParticleSystem.MinMaxGradient(Color.green);
                break;
            case "Blue":
                settings.startColor = new ParticleSystem.MinMaxGradient(Color.blue);
                break;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == this.gameObject.tag)
        {
            particles.gameObject.SetActive(true);
            particles.gameObject.transform.parent = transform.parent;
            particles.gameObject.GetComponent<ParticleDelay>().Display();

            Destroy(this.gameObject);
        }
    }
}
