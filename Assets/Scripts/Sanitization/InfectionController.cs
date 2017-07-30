using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionController : MonoBehaviour
{
    ParticleSystem particles;

    public AudioClip destroy_ding;

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
            case "Teal":
                settings.startColor = new ParticleSystem.MinMaxGradient(Color.cyan);
                break;
            case "Yellow":
                settings.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
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

            transform.parent.gameObject.AddComponent<AudioSource>();
            transform.parent.gameObject.GetComponent<AudioSource>().clip = destroy_ding;
            transform.parent.gameObject.GetComponent<AudioSource>().loop = false;
            transform.parent.gameObject.GetComponent<AudioSource>().Play();

            Destroy(this.gameObject);
        }
    }
}
