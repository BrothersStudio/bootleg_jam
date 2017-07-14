using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float arrow_speed;

    float click_cooldown = 0.2f;
    float last_click = 0f;

    int score = 0;

	void Update ()
    {
        if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > last_click)
        {
            last_click = Time.timeSinceLevelLoad + click_cooldown;

            GetComponent<Rigidbody>().AddForce(-transform.forward * arrow_speed);
        }

        //GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, Mathf.Clamp(GetComponent<Rigidbody>().velocity.y, -200f, 200f));
    }

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log(score++);
    }
}
