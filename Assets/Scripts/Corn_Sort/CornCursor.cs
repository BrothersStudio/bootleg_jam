using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornCursor : MonoBehaviour
{
    public Camera corn_camera;

    bool scoop;

    string corn_tag = "Corn";
    string bug_tag = "Bug";

    void Update()
    {
        // Move circle cursor
        Vector3 mouse_pos = corn_camera.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = transform.position.z;
        transform.position = mouse_pos;

        // Scoop things
        if (Input.GetMouseButton(0))
        {
            scoop = true;
        }
        else
        {
            scoop = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (scoop)
        {
            if (collision.gameObject.CompareTag(bug_tag) || collision.gameObject.CompareTag(corn_tag))
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}
