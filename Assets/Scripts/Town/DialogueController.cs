using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    public bool in_talk_range = false;

    public List<string> dialogue;

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

    public string GetNextLine()
    {
        if (dialogue.Count > 0)
        {
            string next_line = dialogue[0];
            dialogue.RemoveAt(0);
            return next_line;
        }
        else
        {
            return null;
        }
    }
}
