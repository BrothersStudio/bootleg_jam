using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeastController : MonoBehaviour
{
    public float yeast_speed;

    void Update ()
    {
        Camera cam = Camera.main;

        // Rotate yeast to face mouse pointer
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        if (Input.GetMouseButton(0))
        {

        }
        if (Input.GetMouseButton(1))
        {
            Vector3 mouse_pos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouse_pos.z = 0;

            GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(new Vector3(mouse_pos.x - transform.position.x, mousePos.y - transform.position.y, 0f)) * yeast_speed);
        }
	}
}
