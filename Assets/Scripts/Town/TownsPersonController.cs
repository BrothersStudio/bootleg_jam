using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TownsPersonController : MonoBehaviour
{
    MainController main_controller;

    public List<int> dialogue_id;
    [HideInInspector]
    public List<Dialogue> dialogue_lines;
    int dialogue_ind = 0;

    float next_talk = 0f;
    float talk_cooldown = 0.5f;
    string slow_string;

    AudioSource source;
    public AudioClip speaking_clip;

    void Start()
    {
        if (SceneManager.sceneCount > 1)
        {
            GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
            for (int i = 0; i < main_objects.Length; i++)
            {
                if (main_objects[i].name == "MainController")
                {
                    main_controller = main_objects[i].GetComponent<MainController>();
                }
            }
        }

        source = GetComponent<AudioSource>();

        switch (main_controller.current_difficulty)
        {
            case 0:
                dialogue_id.Add(10);
                break;
            case 1:
                dialogue_id.Add(10);
                break;
            case 2:
                dialogue_id.Add(10);
                break;
        }

        for (int i = 0; i < dialogue_id.Count; i++)
        {
            dialogue_lines.Add(DialogueManager.current.GetDialogue(dialogue_id[i]));
        }

        next_talk = Time.timeSinceLevelLoad + talk_cooldown;
        AnimateText(dialogue_lines[dialogue_ind].line);
    }

    void AnimateText(string str)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTextRoutine(str));
    }

    IEnumerator AnimateTextRoutine(string strComplete)
    {
        slow_string = "";
        int i = 0;
        while (i < strComplete.Length)
        {
            source.clip = speaking_clip;
            source.Play();

            slow_string += strComplete[i++];
            GetComponentInChildren<Text>().text = slow_string;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public void NextText()
    {
        if (dialogue_ind < dialogue_lines.Count)
        {
            dialogue_ind++;
            AnimateText(dialogue_lines[dialogue_ind].line);
        }
    }


}