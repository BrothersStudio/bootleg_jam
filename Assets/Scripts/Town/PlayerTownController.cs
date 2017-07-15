using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTownController : MonoBehaviour
{
    public float player_speed = 1f;
    public LayerMask ground_mask;
    public Image dialogue_box;

    bool speaking;
    float next_talk;
    float talk_cooldown = 0.3f;
    Camera cam;
    Vector3 dest;

	void Start ()
    {
        speaking = false;
        next_talk = 0f;

        cam = Camera.main;

        dest = transform.position;
	}

    void FixedUpdate()
    {
        // Player travel
        if (Input.GetMouseButton(1) && !speaking)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, ground_mask))
            {
                Debug.DrawLine(ray.origin, hit.point);
                dest = hit.point;
                dest.y = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, dest, player_speed * Time.deltaTime);

        // Player dialogue
        if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > next_talk)
        {
            next_talk = Time.timeSinceLevelLoad + talk_cooldown;
            Talk();
        }
    }

    public void Talk()
    {
        DialogueController dialogue_controller = GetComponentInChildren<DialogueController>();
        if (dialogue_controller.in_talk_range)
        {
            speaking = true;
            string line = dialogue_controller.GetNextLine();
            if (line != null)
            {
                dialogue_box.GetComponentInChildren<Text>().text = line;
                dialogue_box.gameObject.SetActive(true);
            }
            else
            {
                speaking = false;
                dialogue_box.gameObject.SetActive(false);
            }
        }
    }
}
