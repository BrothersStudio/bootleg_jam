using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    CornGameController controller;

    void Start()
    {
        controller = GameObject.Find("CornGameController").GetComponent<CornGameController>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name != "Cursor")
        {
            if (collision.tag == "Bug")
            {
                controller.game_score--;
            }
            collision.gameObject.SetActive(false);
        }
    }
}
