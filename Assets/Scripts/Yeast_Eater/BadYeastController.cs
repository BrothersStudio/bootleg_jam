using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadYeastController : MonoBehaviour
{
    YeastGameController controller;
    GameObject player;
    public float enemy_speed = 40f;

    float swim_rate = 0.01f;
    float next_swim = 0f;

    float power_attack_rate = 6f;
    float next_power_attack = 0f;

	void Start ()
    {
        player = GameObject.Find("Yeast");

        controller = GameObject.Find("YeastGameController").GetComponent<YeastGameController>();
    }

    void FixedUpdate()
    {
        // Rotate yeast to face player
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, new Vector3(0, 1, 0));
        transform.Rotate(0, -90, 0);

        if (controller.started)
        {
            if (Time.timeSinceLevelLoad > next_swim)
            {
                next_swim = Time.timeSinceLevelLoad + swim_rate;

                GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z)) * enemy_speed);
            }

            if (Vector3.Distance(transform.position, player.transform.position) < 5f && Time.timeSinceLevelLoad > next_power_attack)
            {
                next_power_attack = Time.timeSinceLevelLoad + power_attack_rate;

                GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z)) * enemy_speed * 3f);
            }
            else if (Time.timeSinceLevelLoad > next_power_attack - 5f)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity / 2f;
            }
        }
    }
}
