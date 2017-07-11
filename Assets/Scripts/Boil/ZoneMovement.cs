using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMovement : MonoBehaviour
{
    public float zone_speed;
    float orig_zone_speed;
    bool zoom_mode;

    public float flip_cooldown = 2f;
    float last_flip = 0f;

    Vector3 dest;
    Vector3 top_pos = new Vector3(-13f, 188.81f, 0f);
    Vector3 bot_pos = new Vector3(-13f, -172.03f, 0f);
    Vector3 last_pos;

    void Start()
    {
        orig_zone_speed = zone_speed;

        dest = bot_pos;
        last_pos = GetComponent<RectTransform>().localPosition;
    }

    void Update()
    {
        GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GetComponent<RectTransform>().localPosition, dest, Time.deltaTime * zone_speed);

        if (zoom_mode && GetComponent<RectTransform>().localPosition == dest)
        {
            zoom_mode = false;
            zone_speed = orig_zone_speed;

            dest = bot_pos;
        }

        if (GetComponent<RectTransform>().localPosition == last_pos)
        {
            zoom_mode = true;
            zone_speed = zone_speed * 2;

            dest = new Vector3(-13f, Random.Range(bot_pos.y, top_pos.y), 0f);
        }

		if (Time.timeSinceLevelLoad > last_flip)
        {
            last_flip = Time.timeSinceLevelLoad + flip_cooldown + Random.Range(-1f, 1f);

            zone_speed = zone_speed * Random.Range(0.7f, 1.5f);

            if (dest == top_pos)
                dest = bot_pos;
            else if (dest == bot_pos)
                dest = top_pos;
        }
        last_pos = GetComponent<RectTransform>().localPosition;
    }
}
