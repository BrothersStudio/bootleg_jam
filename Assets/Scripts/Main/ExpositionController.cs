using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpositionController : MonoBehaviour
{
    float next_talk = 0.5f;
    float talk_cooldown = 0.5f;

    public MainController main;
    TownsPersonController dialogue_holder;
    public Text exposition_box;

    void Start()
    {
        dialogue_holder = GameObject.Find("DialogueManager").gameObject.GetComponent<TownsPersonController>();
        Talk();
    }

    void Update ()
    {
        if (Input.GetMouseButton(0) && Time.timeSinceLevelLoad > next_talk)
        {
            next_talk = Time.timeSinceLevelLoad + talk_cooldown;

            if (dialogue_holder.dialogue_lines.Count > 0)
            {
                Talk();
            }
            else
            {
                main.RunNext();
            }

        }
    }

    void Talk ()
    {
        AnimateText(dialogue_holder.dialogue_lines[0].line);
        dialogue_holder.dialogue_lines.RemoveAt(0);
    }

    void AnimateText(string str)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTextRoutine(str));
    }

    IEnumerator AnimateTextRoutine(string strComplete)
    {
        string slow_string = "";
        int i = 0;
        while (i < strComplete.Length)
        {
            slow_string += strComplete[i++];
            exposition_box.GetComponentInChildren<Text>().text = slow_string;
            yield return new WaitForSeconds(0.03f);
        }
    }

}
