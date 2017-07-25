using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool debug = false;
    public int current_difficulty = 0;

    [HideInInspector]
    public float main_time = 0f;

    [HideInInspector]
    public bool sanitization_done, corn_done, yeast_done, boil_done, results_done;

    [HideInInspector]
    public int sanitization_score, corn_score, yeast_score, boil_score, amount_produced;

    [HideInInspector]
    public List<bool> sanitization_upgrades, corn_upgrades, yeast_upgrades, boil_upgrades;

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
        sanitization_upgrades = new List<bool>(new bool[] { false, false });
        corn_upgrades = new List<bool>(new bool[] { false });
        yeast_upgrades = new List<bool>(new bool[] { false, false });
        boil_upgrades = new List<bool>(new bool[] { false });

        if (!debug)
        {
            exposition_screen.SetActive(true);
        }
        else
        {
            RunNext();
        }
    }

    public void RunNext()
    {
        if (!sanitization_done)
        {
            ResetFields();
            current_difficulty++;

            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync("Town");
            }

            SceneManager.LoadScene("Sanitization", LoadSceneMode.Additive);
            exposition_screen.SetActive(false);
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
            amount_produced = 5 + (int)((sanitization_score + corn_score + yeast_score + boil_score) / 16f);
            results_controller.ExecuteResults();
            results_done = true;
        }
        else
        {
            event_system.SetActive(false);
            results_screen.SetActive(false);
            SceneManager.LoadScene("Town", LoadSceneMode.Additive);
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