using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMovement : MonoBehaviour
{
    BoilController controller;

    public float zone_speed;
    float orig_zone_speed;
    bool zoom_mode;

    public float flip_cooldown = 2f;
    float last_flip = 0f;

    Vector3 dest;
    Vector3 top_pos = new Vector3(4.5f, 4.15f, -1.03f);
    Vector3 bot_pos = new Vector3(4.5f, -4.8f, -1.03f);
    Vector3 last_pos;

    void Start()
    {
        orig_zone_speed = zone_speed;

        dest = bot_pos;
        last_pos = transform.position;

        controller = GameObject.Find("BoilController").GetComponent<BoilController>();
    }

    void Update()
    {
        if (controller.started)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, Time.deltaTime * zone_speed);

            if (zoom_mode && transform.position == dest)
            {
                zoom_mode = false;
                zone_speed = orig_zone_speed;

                dest = bot_pos;
            }

            if (transform.position == last_pos)
            {
                zoom_mode = true;
                zone_speed = zone_speed * 2;

                dest = new Vector3(4.5f, Random.Range(bot_pos.y, top_pos.y), -1.03f);
            }

            if (Time.timeSinceLevelLoad > last_flip)
            {
                last_flip = Time.timeSinceLevelLoad + flip_cooldown + Random.Range(-1f, 1f);

                zone_speed = zone_speed * Random.Range(0.7f, 1.2f);

                if (dest == top_pos)
                    dest = bot_pos;
                else if (dest == bot_pos)
                    dest = top_pos;
            }
            last_pos = transform.position;
        }
    }
}
