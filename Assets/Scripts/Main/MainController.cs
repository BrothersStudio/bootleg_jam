using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool debug = false;
    public int current_difficulty = 1;

    [HideInInspector]
    public float main_time = 0f;

    [HideInInspector]
    public bool sanitization_done, corn_done, yeast_done, boil_done, results_done;

    [HideInInspector]
    public int sanitization_score, corn_score, yeast_score, boil_score, amount_produced;

    public GameObject exposition_screen;
    public GameObject results_screen;
    public GameObject event_system;
    public ResultsController results_controller;

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

        amount_produced = 0;
    }

    void Start()
    {
        ResetFields();
        if (!debug)
        {
            exposition_screen.SetActive(true);
        }
    }

    public void RunNext()
    {
        exposition_screen.SetActive(false);

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
            SceneManager.LoadScene("Corn_Sort", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Sanitization");
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
            SceneManager.UnloadSceneAsync("Boil");
            event_system.SetActive(true);
            results_screen.SetActive(true);
            results_controller.ExecuteResults();
            amount_produced = sanitization_score + corn_score + yeast_score + boil_score;
            results_done = true;
        }
        else
        {
            event_system.SetActive(false);
            results_screen.SetActive(false);
            SceneManager.LoadScene("Town", LoadSceneMode.Additive);

            ResetFields();
            current_difficulty++;
        }
    }

    void Update()
    {
        main_time += Time.deltaTime;
    }
}

public enum Upgrade
{
    Sanitization,
    Sort,
    Yeast,
    Boil,
};