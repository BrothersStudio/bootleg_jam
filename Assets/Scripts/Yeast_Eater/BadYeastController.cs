using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadYeastController : MonoBehaviour
{
    YeastGameController controller;
    GameObject player;
    public float enemy_speed;

    float swim_rate = 0.15f;
    float next_swim = 0f;

    int slow_down = 0;
    float power_attack_rate = 8f;
    float next_power_attack = 4f;

    AudioSource source;
    public AudioClip charge_clip;
    public AudioClip[] idle_clip;

	void Start ()
    {
        player = GameObject.Find("Yeast");

        controller = GameObject.Find("YeastGameController").GetComponent<YeastGameController>();

        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Rotate yeast to face player
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, new Vector3(0, 1, 0));
        transform.Rotate(0, -90, 0);

        if (controller.started)
        {
            // Play idle clips
            if (!source.isPlaying)
            {
                source.clip = idle_clip[Random.Range(0, idle_clip.Length)];
                source.Play();
            }

            // Normal swim
            if (Time.timeSinceLevelLoad > next_swim)
            {
                next_swim = Time.timeSinceLevelLoad + swim_rate;

                GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z)) * enemy_speed);
            }

            // Power attack
            if (controller.difficulty > 4)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < 5f && Time.timeSinceLevelLoad > next_power_attack)
                {
                    next_power_attack = Time.timeSinceLevelLoad + power_attack_rate;
                    slow_down = 0;

                    source.clip = charge_clip;
                    source.Play();

                    GetComponent<Rigidbody>().drag = 0.6f;
                    GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z)) * enemy_speed * 70f);
                }
                else if (Time.timeSinceLevelLoad > next_power_attack - 4.5f && slow_down < 5)
                {
                    GetComponent<Rigidbody>().drag = 0.1f;
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity / 2f;
                    slow_down++;
                }
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
    }
}
