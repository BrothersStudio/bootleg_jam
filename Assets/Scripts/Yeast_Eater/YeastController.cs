using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeastController : MonoBehaviour
{
    public float yeast_speed;

    Animator anim;

    bool chomping = false;
    float next_chomp;
    float chomp_cooldown = 1f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate ()
    {
        Camera cam = Camera.main;

        // Rotate yeast to face mouse pointer
        Vector3 mouse_pos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = 0;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mouse_pos - transform.position);

        if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > next_chomp)
        {
            next_chomp = Time.timeSinceLevelLoad + chomp_cooldown;

            chomping = true;
            SetCircleColliders(true);

            anim.SetTrigger("chomp");
            GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(new Vector3(mouse_pos.x - transform.position.x, mouse_pos.y - transform.position.y, 0f)) * yeast_speed * 3f);
        }
        else if (Time.timeSinceLevelLoad > next_chomp)
        {
            chomping = false;
            SetCircleColliders(false);
        }

        if (Input.GetMouseButton(1))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(new Vector3(mouse_pos.x - transform.position.x, mouse_pos.y - transform.position.y, 0f)) * yeast_speed);
        }
	}

    void SetCircleColliders(bool setter)
    {
        GetComponent<CapsuleCollider2D>().enabled = !setter;

        foreach (CircleCollider2D circle in GetComponents<CircleCollider2D>())
        {
            circle.enabled = setter;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        other.gameObject.GetComponent<SugarController>().Shrink();
    }
}
