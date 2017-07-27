using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float arrow_speed;

    float click_cooldown = 0.2f;
    float last_click = 0f;

    BoilController controller;

    AudioSource arrow_audio;
    public AudioClip good_bell;
    public AudioClip wall_bounce;

    void Start()
    {
        controller = GameObject.Find("BoilController").GetComponent<BoilController>();
        arrow_audio = GetComponent<AudioSource>();
    }

    void Update ()
    {
        if (controller.started)
        {
            if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > last_click)
            {
                last_click = Time.timeSinceLevelLoad + click_cooldown;

                GetComponent<Rigidbody>().AddForce(-transform.forward * arrow_speed);
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        controller.game_score++;

        if (!arrow_audio.isPlaying)
        {
            Debug.Log("Here");
            arrow_audio.clip = good_bell;
            arrow_audio.Play();
        }
    }
}
