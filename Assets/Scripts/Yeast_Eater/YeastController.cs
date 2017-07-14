using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeastController : MonoBehaviour
{
    public float yeast_speed;

    bool chomping = false;
    float next_chomp = 0f;
    float chomp_cooldown = 1f;

    void FixedUpdate ()
    {
        Camera cam = Camera.main;

        // Rotate yeast to face mouse pointer lmao sorry about this math
        Vector3 mouse_pos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.y = 0;
        transform.rotation = Quaternion.LookRotation(mouse_pos - transform.position, new Vector3(0, 1, 0));
        transform.Rotate(0, -90, 0);

        if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > next_chomp)
        {
            next_chomp = Time.timeSinceLevelLoad + chomp_cooldown;

            chomping = true;
            SetSphereColliders(true);

            GetComponent<Animation>().Play("Eating");
            GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new Vector3(mouse_pos.x - transform.position.x, 0f, mouse_pos.z - transform.position.z)) * yeast_speed * 2f);
        }
        else if (Time.timeSinceLevelLoad > next_chomp)
        {
            chomping = false;
            SetSphereColliders(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new Vector3(mouse_pos.x - transform.position.x, 0f, mouse_pos.z - transform.position.z)) * yeast_speed);
        }
	}

    void SetSphereColliders(bool setter)
    {
        GetComponent<CapsuleCollider>().enabled = !setter;

        foreach (SphereCollider circle in GetComponents<SphereCollider>())
        {
            circle.enabled = setter;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Wall")
            other.gameObject.GetComponent<SugarController>().Shrink();
    }
}
