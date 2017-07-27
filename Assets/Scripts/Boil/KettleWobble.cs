using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleWobble : MonoBehaviour {

    float wobble_cooldown = 0.01f;
    float next_wobble = 0f;

    public BoilController controller;

    bool fire_sound_played = false;

    void Update ()
    {
        if (controller.started)
        {
            if (!fire_sound_played)
            {
                GetComponent<AudioSource>().Play();
                fire_sound_played = true;
            }

            if (Time.timeSinceLevelLoad > next_wobble)
            {
                next_wobble = Time.timeSinceLevelLoad + wobble_cooldown;

                transform.Rotate(Random.Range(-0.25f, 0.25f), 0f, 0f);
            }
        }
	}
}
