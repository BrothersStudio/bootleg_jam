using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeastController : MonoBehaviour
{
    YeastGameController controller;

    public float yeast_speed;
    public Camera yeast_cam;

    float next_chomp = 0f;
    float chomp_cooldown = 0.75f;

    AudioSource chomp_source;
    AudioSource swim_source;
    AudioSource fart_source;
    public AudioClip chomp_clip;
    public AudioClip swim_clip;
    public AudioClip fart_clip;

    void Start()
    {
        GetComponent<Animation>().Play("Idle");

        controller = GameObject.Find("YeastGameController").GetComponent<YeastGameController>();

        AudioSource[] audio_sources = GetComponents<AudioSource>();
        chomp_source = audio_sources[0];
        swim_source = audio_sources[1];
        fart_source = audio_sources[2];
    }

    void Update()
    {
        if (controller.started)
        {
            Vector3 mouse_pos = yeast_cam.ScreenToWorldPoint(Input.mousePosition);
            mouse_pos.y = 0;

            // Swim
            if (Input.GetMouseButtonDown(1))
            {
                swim_source.clip = swim_clip;
                chomp_source.pitch = Random.Range(0.8f, 1.2f);
                swim_source.Play();

                GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new Vector3(mouse_pos.x - transform.position.x, 0f, mouse_pos.z - transform.position.z)) * yeast_speed);
            }

            if (Time.timeSinceLevelLoad > next_chomp)
            {
                next_chomp = Time.timeSinceLevelLoad + chomp_cooldown;

                GetComponent<Animation>().Play("Eating");
            }

            var shape = GetComponentInChildren<ParticleSystem>().shape;
            if (GetComponent<Rigidbody>().velocity.magnitude > 5)
            {
                shape.angle = 10f;
            }
            else
            {
                shape.angle = 50f;
            }
        }
        else if (!GetComponent<Animation>().isPlaying)
        {
            GetComponent<Animation>().Play("Idle");
        }
    }

    void FixedUpdate ()
    {
        // Rotate yeast to face mouse pointer lmao sorry about this math
        Vector3 mouse_pos = yeast_cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.y = 0;
        transform.rotation = Quaternion.LookRotation(mouse_pos - transform.position, new Vector3(0, 1, 0));
        transform.Rotate(0, -90, 0);

        GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, yeast_speed * 3);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Wall")
        {
            other.gameObject.GetComponent<SugarController>().Shrink();

            fart_source.clip = fart_clip;
            fart_source.pitch = Random.Range(0.6f, 1.6f);
            fart_source.Play();
        }
    }
}
