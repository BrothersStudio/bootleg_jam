using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueController : MonoBehaviour {

    [HideInInspector]
    public bool in_talk_range = false;
    [HideInInspector]
    public bool in_exit_zone = false;

    public List<Dialogue> dialogue;

    public int dialogue_ind;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Townsperson")
        {
            dialogue_ind = 0;
            in_talk_range = true;
        }
        else if (other.tag == "Exit Zone")
        {
            dialogue_ind = 0;
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
            dialogue_ind = 0;
            in_talk_range = false;
            dialogue = new List<Dialogue>();
        }
        else if (other.tag == "Exit Zone")
        {
            dialogue_ind = 0;
            in_exit_zone = false;
            dialogue = new List<Dialogue>();
        }
    }

    public Dialogue GetNextDialogue()
    {
        if (dialogue_ind <= dialogue.Count - 1)
        {
            Dialogue next_dialogue = dialogue[dialogue_ind];
            dialogue_ind++;
            return next_dialogue;
        }
        else
        {
            return null;
        }
    }
}
