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
        AnimateNumber(sanitization_text, main_con.sanitization_score);
        AnimateNumber(sort_text, main_con.corn_score);
        AnimateNumber(yeast_text, main_con.yeast_score);
        AnimateNumber(boil_text, main_con.boil_score);
        AnimateNumber(total_text, main_con.sanitization_score + main_con.corn_score + main_con.yeast_score + main_con.boil_score, 0.005f);
    }

    void AnimateNumber(Text box, int score, float speed = 0.02f)
    {
        StartCoroutine(AnimateNumberRoutine(box, score, speed));
    }

    IEnumerator AnimateNumberRoutine(Text box, int score_complete, float speed = 0.02f)
    {
        int i = 0;
        while (i < score_complete + 1)
        {
            box.GetComponentInChildren<Text>().text = i++.ToString();
            yield return new WaitForSeconds(speed);
        }
    }
}
