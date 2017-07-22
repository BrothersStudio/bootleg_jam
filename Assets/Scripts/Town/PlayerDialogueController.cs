using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueController : MonoBehaviour {

    [HideInInspector]
    public bool in_talk_range = false;
    [HideInInspector]
    public bool in_exit_zone = false;

    public List<Dialogue> dialogue;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Townsperson")
        {
            in_talk_range = true;
        }
        else if (other.tag == "Exit Zone")
        {
            in_exit_zone = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Townsperson")
        {
            in_talk_range = true;
            dialogue = other.GetComponent<TownsPersonController>().dialogue_lines;
        }
        else if (other.tag == "Exit Zone")
        {
            in_exit_zone = true;
            dialogue = other.GetComponent<TownsPersonController>().dialogue_lines;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Townsperson")
        {
            in_talk_range = false;
            dialogue.Clear();
        }
        else if (other.tag == "Exit Zone")
        {
            in_exit_zone = false;
            dialogue.Clear();
        }
    }

    public Dialogue GetNextDialogue()
    {
        if (dialogue.Count > 0)
        {
            Dialogue next_dialogue = dialogue[0];
            dialogue.RemoveAt(0);
            return next_dialogue;
        }
        else
        {
            return null;
        }
    }
}
