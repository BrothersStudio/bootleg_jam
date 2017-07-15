using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KettleController : MonoBehaviour
{
    public float kettle_speed;
    public float jump_power;

    public Transform groundCheck;
    float groundRadius = 0.3f;
    public LayerMask whatIsGround;

    Rigidbody rigid;

    void Start ()
    {
        rigid = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        if (IsGrounded() && Input.GetMouseButton(0))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0f, jump_power), ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        rigid.AddForce(Vector3.right * kettle_speed);
        Vector3 current_velocity = GetComponent<Rigidbody>().velocity;
        float new_x = Mathf.Clamp(current_velocity.x, 0, 20f);
        current_velocity.x = new_x;

        GetComponent<Rigidbody>().velocity = current_velocity;
    }

    bool IsGrounded()
    {
        Collider[] finds = Physics.OverlapSphere(groundCheck.position, groundRadius, whatIsGround);
        if (finds.Length > 0)
            return true;
        else
            return false;
    }
}
