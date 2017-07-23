using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TownsPersonController : MonoBehaviour
{
    public int[] dialogue_id;

    [HideInInspector]
    public List<Dialogue> dialogue_lines;

    void Start()
    {
        Debug.Log("Loading stuff!");
        for (int i = 0; i < dialogue_id.Length; i++)
        {
            dialogue_lines.Add(DialogueManager.current.GetDialogue(dialogue_id[i]));
        }
    }
}