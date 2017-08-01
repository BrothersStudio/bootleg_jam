using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleController : MonoBehaviour
{
    SanitizationController controller;

    public float kettle_speed;

    bool frozen = false;
    Vector3 old_velocity;

    Rigidbody rigid;

    void Start ()
    {
        controller = GameObject.Find("GameController").GetComponent<SanitizationController>();

        rigid = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
        if (controller.freeze)
        {
            old_velocity = rigid.velocity;
            rigid.velocity = new Vector3(0f, 0f, 0f);
            frozen = true;
            return;
        }

        if (frozen)
        {
            rigid.velocity = old_velocity;
            frozen = false;
        }

        if (transform.position.y < 0.1f)
        {
            rigid.velocity = Vector3.right * kettle_speed;
        }
    } 
}
