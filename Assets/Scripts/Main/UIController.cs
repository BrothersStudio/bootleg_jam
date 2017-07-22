using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public MainController main_con;

    public Text sanitization_text, sort_text, yeast_text, boil_text, total_text;

	public void ExecuteResults()
    {
        AnimateText(sanitization_text, main_con.sanitization_score + 1);
        AnimateText(sort_text, main_con.corn_score + 1);
        AnimateText(yeast_text, main_con.yeast_score + 1);
        AnimateText(boil_text, main_con.boil_score + 1);
        AnimateText(total_text, main_con.sanitization_score + main_con.corn_score + main_con.yeast_score + main_con.boil_score + 4);
    }

    void AnimateText(Text box, int score)
    {
        StartCoroutine(AnimateTextRoutine(box, score));
    }

    IEnumerator AnimateTextRoutine(Text box, int score_complete)
    {
        int i = 0;
        while (i < score_complete)
        {
            box.GetComponentInChildren<Text>().text = i++.ToString();
            yield return new WaitForSeconds(0.02F);
        }
    }
}
