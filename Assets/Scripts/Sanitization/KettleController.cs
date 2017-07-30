using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleController : MonoBehaviour
{
    SanitizationController controller;

    public float kettle_speed;

    Rigidbody rigid;

    void Start ()
    {
        controller = GameObject.Find("GameController").GetComponent<SanitizationController>();

        rigid = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        if (!controller.freeze)
        {
            if (transform.position.y < 5f)
            {
                rigid.AddForce(Vector3.right * kettle_speed);
                Vector3 current_velocity = GetComponent<Rigidbody>().velocity;
                float new_x = Mathf.Clamp(current_velocity.x, 0, kettle_speed);
                current_velocity.x = new_x;

                GetComponent<Rigidbody>().velocity = current_velocity;
            }
        }
    } 
}
