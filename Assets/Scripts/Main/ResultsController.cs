using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsController : MonoBehaviour
{
    public MainController main_con;
    AudioSource result_source;

    public Text sanitization_text, sort_text, yeast_text, boil_text, total_text;

    void Start()
    {
        result_source = GetComponent<AudioSource>();
    }

    public void ExecuteResults()
    {
        sanitization_text.text = "0";
        sort_text.text = "0";
        yeast_text.text = "0";
        boil_text.text = "0";
        total_text.text = main_con.total_amount.ToString();

        AnimateNumber(sanitization_text, 0, main_con.sanitization_score, true);
        AnimateNumber(sort_text, 0, main_con.corn_score);
        AnimateNumber(yeast_text, 0, main_con.yeast_score);
        AnimateNumber(boil_text, 0, main_con.boil_score);


        AnimateNumber(total_text, main_con.total_amount, main_con.total_amount + main_con.amount_produced, true, 3f);
    }

    void AnimateNumber(Text box, int orig, int score, bool sound = false, float delay = 0)
    {
        StartCoroutine(AnimateNumberRoutine(box, orig, score, delay, sound));
    }

    IEnumerator AnimateNumberRoutine(Text box, int orig, int score_complete, float delay, bool sound)
    {
        yield return new WaitForSeconds(delay);

        int i = orig;
        while (i < score_complete + 1)
        {
            if (sound)
            {
                result_source.Play();
            }

            box.GetComponentInChildren<Text>().text = i++.ToString();
            yield return new WaitForSeconds(0.02f);
        }
    }
}
