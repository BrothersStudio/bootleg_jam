using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerTownController : MonoBehaviour
{
    MainController main_controller;
    int amount_produced = 999;
    public Text amount_box;

    public float player_speed = 1f;
    public LayerMask person_mask;
    public LayerMask ground_mask;

    AudioSource player_source;
    public AudioClip[] footsteps_clips;
    public AudioClip speaking_clip;
    public AudioClip coin_clip;

    string slow_string;
    public GameObject dialogue_box;
    public GameObject choice_box;
    public Camera town_cam;

    bool speaking;
    float next_talk;
    float talk_cooldown = 0.5f;
    Vector3 dest;

	void Start ()
    {
        speaking = false;
        next_talk = 0f;

        dest = transform.position;

        player_source = GetComponent<AudioSource>();

        if (SceneManager.sceneCount > 1)
        {
            GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
            for (int i = 0; i < main_objects.Length; i++)
            {
                if (main_objects[i].name == "MainController")
                {
                    main_controller = main_objects[i].GetComponent<MainController>();
                    amount_produced = main_controller.amount_produced;

                    main_controller.camera_listener.enabled = false;
                    main_controller.event_system.SetActive(false);
                    main_controller.results_screen.SetActive(false);
                }
            }
        }

        amount_box.text = "Current Amount:\n" + amount_produced.ToString();
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
            Ray ray = town_cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, person_mask))
            {
                return;
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground_mask))
            {
                Debug.DrawLine(ray.origin, hit.point);
                dest = hit.point;
                dest.y = 0;
            }
        }

        // Footsteps
        if (Vector3.Distance(transform.position, dest) > 0 && !player_source.isPlaying)
        {
            player_source.clip = footsteps_clips[Random.Range(0, footsteps_clips.Length)];
            player_source.Play();
        }
    }

    public void Talk()
    {
        PlayerDialogueController dialogue_controller = GetComponentInChildren<PlayerDialogueController>();
        if (dialogue_controller.in_talk_range || dialogue_controller.in_exit_zone)
        {
            Dialogue next_dialogue = dialogue_controller.GetNextDialogue();
            if (next_dialogue != null)
            {
                dest = transform.position;
                speaking = true;

                if (next_dialogue.is_choice)
                {
                    AnimateText(choice_box, next_dialogue.line);
                    choice_box.SetActive(true);
                    next_talk = Time.timeSinceLevelLoad + talk_cooldown;

                    Button[] buttons = choice_box.GetComponentsInChildren<Button>();

                    buttons[0].onClick.RemoveAllListeners();
                    buttons[0].onClick.AddListener(() =>
                    {
                        if (dialogue_controller.in_exit_zone)
                        {
                            CloseScene();
                        }
                        else
                        {
                            int previous_amount = amount_produced;
                            amount_produced -= next_dialogue.price;
                            AnimateNumber(previous_amount, amount_produced);
                            choice_box.SetActive(false);
                            dialogue_box.SetActive(true);
                            next_talk = Time.timeSinceLevelLoad + talk_cooldown;

                            if (next_dialogue.yes_line != "")
                            {
                                AnimateText(dialogue_box, next_dialogue.yes_line);
                            }
                            else
                            {
                                Talk();
                            }
                        }
                    });

                    buttons[1].onClick.RemoveAllListeners();
                    buttons[1].onClick.AddListener(() =>
                    {
                        choice_box.SetActive(false);
                        dialogue_box.SetActive(true);
                        next_talk = Time.timeSinceLevelLoad + talk_cooldown;

                        if (next_dialogue.yes_line != "")
                        {
                            AnimateText(dialogue_box, next_dialogue.no_line);
                        }
                        else
                        {
                            Talk();
                        }
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
        else if (dialogue_controller.in_exit_zone)
        {

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
            player_source.clip = speaking_clip;
            player_source.Play();

            slow_string += strComplete[i++];
            box.GetComponentInChildren<Text>().text = slow_string;
            yield return new WaitForSeconds(0.03f);
        }
    }

    void AnimateNumber(int previous_amount, int new_amount)
    {
        StartCoroutine(AnimateNumberRoutine(previous_amount, new_amount));
    }

    IEnumerator AnimateNumberRoutine(int previous_amount, int new_amount)
    {
        int i = previous_amount;
        while (i > new_amount)
        {
            player_source.clip = coin_clip;
            player_source.Play();

            i--;
            amount_box.text = "Current Amount:\n" + i.ToString();
            yield return new WaitForSeconds(0.01f);
        }
    }

    void CloseScene()
    {
        main_controller.RunNext();
    }
}
