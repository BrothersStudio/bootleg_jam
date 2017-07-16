using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueController : MonoBehaviour {

    public bool in_talk_range = false;

    public List<Dialogue> dialogue;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Townsperson")
        {
            in_talk_range = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Townsperson")
        {
            in_talk_range = true;
            dialogue = other.GetComponent<TownsPersonController>().dialogue_lines;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Townsperson")
        {
            in_talk_range = false;
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
