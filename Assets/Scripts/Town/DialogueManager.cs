using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager current;

    Dictionary<int, Dialogue> game_dialogue;

    void Awake()
    {
        current = this;

        TextAsset dialogue_json = Resources.Load("Dialogue/dialogue") as TextAsset;
        AllDialogue loaded_dialogue = JsonUtility.FromJson<AllDialogue>(dialogue_json.text);

        game_dialogue = new Dictionary<int, Dialogue>();
        for (int i = 0; i < loaded_dialogue.all_dialogue.Length; i++)
        {
            game_dialogue.Add(loaded_dialogue.all_dialogue[i].id, loaded_dialogue.all_dialogue[i]);
        }
    }

    public Dialogue GetDialogue(int id)
    {
        return game_dialogue[id];
    }
}

[System.Serializable]
public class AllDialogue
{
    public Dialogue[] all_dialogue;
}

[System.Serializable]
public class Dialogue
{
    public int id = 0;

    public string line = "";

    public bool is_choice = false;
    public int price = 0;
    public bool is_food = false;
    public string yes_line = "";
    public string no_line = "";
}