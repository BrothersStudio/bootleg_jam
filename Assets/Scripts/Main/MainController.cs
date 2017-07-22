﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    [HideInInspector]
    public float main_time = 0f;

    [HideInInspector]
    public bool sanitization_done, corn_done, yeast_done, boil_done, results_done;

    [HideInInspector]
    public int sanitization_score, corn_score, yeast_score, boil_score;

    public GameObject canvas;
    public UIController uicontroller;

    void ResetFields()
    {
        sanitization_done = false;
        corn_done = false;
        yeast_done = false;
        boil_done = false;
        results_done = false;

        sanitization_score = 0;
        corn_score = 0;
        yeast_score = 0;
        boil_score = 0;
    }

    void Start()
    {
        ResetFields();
        RunNext();
    }

    public void RunNext()
    {
        main_time = 0f;

        if (!sanitization_done)
        {
            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync("Town");
            }

            SceneManager.LoadScene("Sanitization", LoadSceneMode.Additive);
        }
        else if (!corn_done)
        {
            SceneManager.UnloadSceneAsync("Sanitization");
            SceneManager.LoadScene("Corn_Sort", LoadSceneMode.Additive);
        }
        else if (!yeast_done)
        {
            SceneManager.LoadScene("Yeast_Eater", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Corn_Sort");
        }
        else if (!boil_done)
        {
            SceneManager.LoadScene("Boil", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Yeast_Eater");
        }
        else if (!results_done)
        {
            canvas.SetActive(true);
            uicontroller.ExecuteResults();
            SceneManager.UnloadSceneAsync("Boil");
            results_done = true;
        }
        else
        {
            canvas.SetActive(false);
            SceneManager.LoadScene("Town", LoadSceneMode.Additive);
            ResetFields();
        }
    }

    private void Update()
    {
        main_time += Time.deltaTime;
    }
}
