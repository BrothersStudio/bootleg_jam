using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornCursor : MonoBehaviour
{
    public Camera corn_camera;

    string corn_tag = "Corn";
    string bug_tag = "Bug";

    CornGameController controller;

    void Start()
    {
        controller = GameObject.Find("CornGameController").GetComponent<CornGameController>();
    }

    void Update()
    {
        // Move circle cursor
        Vector3 mouse_pos = corn_camera.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = transform.position.z;
        transform.position = mouse_pos;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag(corn_tag))
        {
            controller.game_score--;
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag(bug_tag))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
