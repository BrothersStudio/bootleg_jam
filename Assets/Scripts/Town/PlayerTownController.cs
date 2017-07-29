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

    public bool fed;
    bool fed_chance;
    public GameObject results_background;

    bool speaking;
    bool choosing;
    float next_talk;
    float talk_cooldown = 0.5f;
    Vector3 dest;

	void Start ()
    {
        fed = false;
        fed_chance = false;

        speaking = false;
        choosing = false;
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
            if (next_dialogue != null && !choosing)
            {
                dest = transform.position;
                speaking = true;

                if (next_dialogue.is_choice)
                {
                    choosing = true;

                    AnimateText(choice_box, next_dialogue.line);
                    choice_box.SetActive(true);
                    next_talk = Time.timeSinceLevelLoad + talk_cooldown;

                    Button[] buttons = choice_box.GetComponentsInChildren<Button>();

                    buttons[0].onClick.RemoveAllListeners();
                    buttons[0].onClick.AddListener(() =>
                    {
                        choosing = false;

                        if (dialogue_controller.in_exit_zone)
                        {
                            if (!fed && !fed_chance)
                            {
                                fed_chance = true;
                                dialogue_controller.dialogue_ind--;
                                Talk();
                            }
                            else
                            {
                                CloseScene();
                            }
                        }
                        else
                        {
                            int previous_amount = amount_produced;
                            amount_produced -= next_dialogue.price;

                            if (next_dialogue.is_food)
                            {
                                fed = true;
                                fed_chance = true;
                            }

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
                        choosing = false;

                        if (dialogue_controller.in_exit_zone)
                        {
                            EndDialogue();
                        }
                        else
                        {
                            choice_box.SetActive(false);
                            dialogue_box.SetActive(true);
                            next_talk = Time.timeSinceLevelLoad + talk_cooldown;

                            if (next_dialogue.no_line != "")
                            {
                                AnimateText(dialogue_box, next_dialogue.no_line);
                            }
                            else
                            {
                                Talk();
                            }
                        }
                    });
                }
                else
                {
                    next_talk = Time.timeSinceLevelLoad + talk_cooldown;

                    AnimateText(dialogue_box, next_dialogue.line);
                    dialogue_box.SetActive(true);
                }
            }
            else if (!choosing)
            {
                EndDialogue();
            }
        }
    }

    void EndDialogue()
    {
        next_talk = Time.timeSinceLevelLoad + talk_cooldown;
        GetComponentInChildren<PlayerDialogueController>().dialogue_ind = 0;
        speaking = false;
        choosing = false;

        choice_box.SetActive(false);
        dialogue_box.SetActive(false);
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
        results_background.SetActive(true);

        if (fed && ((10 - main_controller.current_difficulty) == 0))
        {
            // Win state
            results_background.transform.Find("Fed Text").gameObject.GetComponent<Text>().text = "You survived prohibition.\nYou are now a wealthy moonshine baron.";
            results_background.transform.Find("Success/Remaining Text").gameObject.SetActive(false);

            results_background.transform.Find("Success").gameObject.SetActive(false);
            results_background.transform.Find("Failure").gameObject.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                main_controller.QuitGame();
            });
        }
        if (fed)
        {
            // Next month state
            results_background.transform.Find("Fed Text").gameObject.GetComponent<Text>().text = "You fed your family this month!";
            results_background.transform.Find("Success/Remaining Text").gameObject.GetComponent<Text>().text = (10 - main_controller.current_difficulty).ToString() + " months to go";

            results_background.transform.Find("Failure").gameObject.SetActive(false);
            results_background.transform.Find("Success").gameObject.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                main_controller.town_done = true;
                main_controller.RunNext();
            });
        }
        else
        {
            // Lose state
            results_background.transform.Find("Fed Text").gameObject.GetComponent<Text>().text = "Game Over!\nYour family has starved and died.\nWhat was this even for...";
            results_background.transform.Find("Success/Remaining Text").gameObject.SetActive(false);

            results_background.transform.Find("Success").gameObject.SetActive(false);
            results_background.transform.Find("Failure").gameObject.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                main_controller.QuitGame();
            });
        }
    }
}
