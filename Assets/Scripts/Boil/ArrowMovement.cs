using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float arrow_speed;

    float click_cooldown = 0.2f;
    float last_click = 0f;

    BoilController controller;

    void Start()
    {
        controller = GameObject.Find("BoilController").GetComponent<BoilController>();
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
    }
}
