using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerTownController : MonoBehaviour
{
    MainController main_controller;

    Vector3 dest;

    AudioSource player_source;
    public AudioClip coin_clip;

    public Button continue_button;
    public Text gallons_text;

    bool fed = false;
    int food_cost = 20;

	void Start ()
    {
        dest = new Vector3(-1.74f, 0f, -7.32f);

        if (SceneManager.sceneCount > 1)
        {
            GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
            for (int i = 0; i < main_objects.Length; i++)
            {
                if (main_objects[i].name == "MainController")
                {
                    main_controller = main_objects[i].GetComponent<MainController>();

                    main_controller.camera_listener.enabled = false;
                    main_controller.event_system.SetActive(false);
                    main_controller.results_screen.SetActive(false);
                }
            }
        }

        continue_button.onClick.AddListener(() =>
        {
            main_controller.town_done = true;
            main_controller.RunNext();
        });

        player_source = GetComponent<AudioSource>();

        gallons_text.text = "Gallons of Booze:\n" + main_controller.total_amount;
    }

    public void BuyFood()
    {

        AnimateNumber(main_controller.total_amount, main_controller.total_amount - food_cost);
        main_controller.total_amount -= food_cost;
        fed = true;
    }

    void AnimateNumber(int previous_amount, int new_amount)
    {
        StartCoroutine(AnimateNumberRoutine(previous_amount, new_amount));
    }

    IEnumerator AnimateNumberRoutine(int previous_amount, int new_amount)
    {
        int i = previous_amount;
        while (i > new_amount)
        {
            player_source.clip = coin_clip;
            player_source.Play();

            i--;
            gallons_text.text = "Gallons of Booze:\n" + i.ToString();
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Update()
    {
        // Rotate to face destination
        if (Vector3.Distance(dest, transform.position) > 1)
        {
            transform.rotation = Quaternion.LookRotation(dest - transform.position, new Vector3(0, 1, 0));
        }

        // Player travel
        transform.position = Vector3.MoveTowards(transform.position, dest, 1f * Time.deltaTime);
    }
}
