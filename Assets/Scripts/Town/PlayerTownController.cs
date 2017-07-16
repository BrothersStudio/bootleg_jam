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
    string slow_string;
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
        // Player dialogue
        if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > next_talk)
        {
            Talk();
        }

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
    }

    public void Talk()
    {
        PlayerDialogueController dialogue_controller = GetComponentInChildren<PlayerDialogueController>();
        if (dialogue_controller.in_talk_range)
        {
            Dialogue next_dialogue = dialogue_controller.GetNextDialogue();
            if (next_dialogue != null)
            {
                dest = transform.position;
                speaking = true;

                if (next_dialogue.isChoice)
                {
                    AnimateText(choice_box, next_dialogue.line);
                    choice_box.SetActive(true);
                    next_talk = Time.timeSinceLevelLoad + talk_cooldown;

                    Button[] buttons = choice_box.GetComponentsInChildren<Button>();

                    buttons[0].onClick.RemoveAllListeners();
                    buttons[0].onClick.AddListener(() =>
                    {
                        AnimateText(dialogue_box, next_dialogue.yes_line);
                        choice_box.SetActive(false);
                        dialogue_box.SetActive(true);
                        next_talk = Time.timeSinceLevelLoad + talk_cooldown;
                    });

                    buttons[1].onClick.RemoveAllListeners();
                    buttons[1].onClick.AddListener(() =>
                    {
                        AnimateText(dialogue_box, next_dialogue.no_line);
                        choice_box.SetActive(false);
                        dialogue_box.SetActive(true);
                        next_talk = Time.timeSinceLevelLoad + talk_cooldown;
                    });
                }
                else
                {
                    AnimateText(dialogue_box, next_dialogue.line);
                    dialogue_box.SetActive(true);
                    next_talk = Time.timeSinceLevelLoad + talk_cooldown;
                }
            }
            else
            {
                speaking = false;
                dialogue_box.SetActive(false);
                next_talk = Time.timeSinceLevelLoad + talk_cooldown;
            }
        }
    }

    void AnimateText(GameObject box, string str)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTextRoutine(box, str));
    }

    IEnumerator AnimateTextRoutine(GameObject box, string strComplete)
    {
        slow_string = "";
        int i = 0;
        while (i < strComplete.Length)
        {
            slow_string += strComplete[i++];
            box.GetComponentInChildren<Text>().text = slow_string;
            yield return new WaitForSeconds(0.05F);
        }
    }
}
