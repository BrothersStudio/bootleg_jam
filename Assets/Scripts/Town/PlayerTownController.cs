using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTownController : MonoBehaviour
{
    public float player_speed = 1f;
    public LayerMask person_mask;
    public LayerMask ground_mask;

    public GameObject dialogue_box;
    public GameObject choice_box;

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
        transform.position = Vector3.MoveTowards(transform.position, dest, player_speed * Time.deltaTime);

        if (Input.GetMouseButton(1) && !speaking)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, person_mask))
            {
                return;
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.DrawLine(ray.origin, hit.point);
                dest = hit.point;
                dest.y = 0;
            }
        }

        // Player dialogue
        if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > next_talk)
        {
            Debug.Log(next_talk);
            Debug.Log(Time.timeSinceLevelLoad);

            dest = transform.position;
            next_talk = Time.timeSinceLevelLoad + talk_cooldown;

            Talk();
        }
    }

    public void Talk()
    {
        PlayerDialogueController dialogue_controller = GetComponentInChildren<PlayerDialogueController>();
        if (dialogue_controller.in_talk_range)
        {
            speaking = true;
            Dialogue next_dialogue = dialogue_controller.GetNextDialogue();
            if (next_dialogue != null)
            {
                if (next_dialogue.isChoice)
                {
                    choice_box.GetComponentInChildren<Text>().text = next_dialogue.line;
                    choice_box.SetActive(true);
                    dialogue_box.SetActive(true);
                    Button[] buttons = choice_box.GetComponentsInChildren<Button>();
                    buttons[0].onClick.RemoveAllListeners();
                    buttons[0].onClick.AddListener(() =>
                    {
                        dialogue_box.GetComponentInChildren<Text>().text = next_dialogue.yes_line;
                        choice_box.SetActive(false);
                    });
                    buttons[1].onClick.RemoveAllListeners();
                    buttons[1].onClick.AddListener(() =>
                    {
                        dialogue_box.GetComponentInChildren<Text>().text = next_dialogue.no_line;
                        choice_box.SetActive(false);
                    });
                }
                else
                {
                    dialogue_box.GetComponentInChildren<Text>().text = next_dialogue.line;
                    dialogue_box.SetActive(true);
                    choice_box.SetActive(false);
                }
            }
            else
            {
                speaking = false;
                dialogue_box.SetActive(false);
            }
        }
    }
}
