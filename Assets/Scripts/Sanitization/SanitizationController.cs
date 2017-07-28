﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SanitizationController : GameControllers
{
    public Camera sanitization_cam;
    public GameObject sanitization_spawn_loc;
    public GameObject reticle;

    public GameObject kettle_prefab;
    public GameObject infection_prefab;
    public Material[] color_mats;

    public LayerMask red_mask;
    public LayerMask green_mask;
    public LayerMask blue_mask;

    int current_selection = -1;
    public AudioClip color_select;

    public float spray_speed;
    public float spray_cooldown;
    float next_spray = 0f;
    public AudioSource spray_source;

    int low_infections_per_kettle;
    int high_infections_per_kettle;
    public float kettle_spawn_period;
    float next_kettle = 0f;

    int total_kettles = 0;
    public int game_score = 0;
    public int difficulty = 1;

    new void Start()
    {
        Cursor.visible = false;

        base.Start();

        if (main_controller != null)
        {
            difficulty = main_controller.current_difficulty;
        }

        SetDifficulty();
        StartCoroutine(StartCountdown("Sanitize!"));

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 23.7f);
        }
    }

    void SetDifficulty()
    {
        if (difficulty == 10)
        {
            low_infections_per_kettle = 3;
            high_infections_per_kettle = 4;

            kettle_spawn_period = 2f;
        }
        else if (difficulty > 8)
        {
            low_infections_per_kettle = 2;
            high_infections_per_kettle = 4;

            kettle_spawn_period = 2.5f;
        }
        else if (difficulty > 6)
        {
            low_infections_per_kettle = 2;
            high_infections_per_kettle = 3;

            kettle_spawn_period = 2.5f;
        }
        else if (difficulty > 4)
        {
            low_infections_per_kettle = 2;
            high_infections_per_kettle = 2;

            kettle_spawn_period = 2.5f;
        }
        else if (difficulty > 2)
        {
            low_infections_per_kettle = 1;
            high_infections_per_kettle = 2;

            kettle_spawn_period = 2.5f;
        }
        else if (difficulty > 1)
        {
            low_infections_per_kettle = 1;
            high_infections_per_kettle = 2;

            kettle_spawn_period = 3f;
        }
        else if (difficulty == 1)
        {
            low_infections_per_kettle = 1;
            high_infections_per_kettle = 1;

            kettle_spawn_period = 3f;
        }
    }

    void Update()
    {
        if (started)
        {
            if (main_controller != null)
            {
                HandleTime(-(main_controller.main_time - 20f));
            }

            HandleDisinfectant();

            SpawnKettles();
        }
    }

    void HandleDisinfectant()
    {
        if (Input.GetMouseButton(0))
        {
            // Select color
            Ray ray = sanitization_cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, red_mask))
            {
                GetComponent<AudioSource>().clip = color_select;
                GetComponent<AudioSource>().Play();
                current_selection = 0;
                return;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, green_mask))
            {
                GetComponent<AudioSource>().clip = color_select;
                GetComponent<AudioSource>().Play();
                current_selection = 1;
                return;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, blue_mask))
            {
                GetComponent<AudioSource>().clip = color_select;
                GetComponent<AudioSource>().Play();
                current_selection = 2;
                return;
            }

            if (Time.timeSinceLevelLoad > next_spray && current_selection >= 0)
            {
                next_spray = Time.timeSinceLevelLoad + spray_cooldown;
                spray_source.Play();

                for (int i = 0; i < 20; i++)
                {
                    GameObject disinfectant = DisinfectantPool.current.GetPooledDisinfectant();
                    Vector3 new_pos = sanitization_spawn_loc.transform.position;
                    new_pos.x += Random.Range(-1.5f, 1.5f);
                    new_pos.y += Random.Range(-1.5f, 1.5f);
                    new_pos.z += Random.Range(-0.2f, 2f);
                    disinfectant.transform.position = new_pos;

                    Vector3 velocity_direction = sanitization_spawn_loc.transform.forward;
                    velocity_direction.x += Random.Range(-0.04f, 0.04f);
                    velocity_direction.y += Random.Range(-0.04f, 0.04f);
                    velocity_direction.z += Random.Range(-0.04f, 0.04f);

                    disinfectant.GetComponent<Rigidbody>().velocity = velocity_direction * spray_speed;

                    disinfectant.GetComponent<MeshRenderer>().material = color_mats[current_selection];
                    disinfectant.SetActive(true);

                    switch (current_selection)
                    {
                        case 0:
                            disinfectant.tag = "Red";
                            break;
                        case 1:
                            disinfectant.tag = "Green";
                            break;
                        case 2:
                            disinfectant.tag = "Blue";
                            break;
                    }
                }
            }
        }
    }

    void SpawnKettles()
    {
        if (Time.timeSinceLevelLoad > next_kettle)
        {
            total_kettles++;
            next_kettle = Time.timeSinceLevelLoad + kettle_spawn_period;

            GameObject kettle = Instantiate(kettle_prefab, new Vector3(-34f, 25f, -2.2f), Quaternion.identity, transform);
            kettle.transform.Rotate(new Vector3(-90f, 0f, 0f));

            int infection_num = Random.Range(low_infections_per_kettle, high_infections_per_kettle + 1);
            for (int i = 0; i < infection_num; i++)
            {
                GameObject infection = Instantiate(infection_prefab, kettle.transform);
                infection.name = "Infection";
                infection.transform.localPosition = new Vector3(Random.Range(-1f, 1f), 1.62f, Random.Range(-0.5f, 1.5f));

                int infection_roll = Random.Range(0, color_mats.Length);
                infection.GetComponent<MeshRenderer>().material = color_mats[infection_roll];
                switch (infection_roll)
                {
                    case 0:
                        infection.tag = "Red";
                        break;
                    case 1:
                        infection.tag = "Green";
                        break;
                    case 2:
                        infection.tag = "Blue";
                        break;
                }
            }
        }
    }

    new void EndScene()
    {
        Cursor.visible = true;
        game_score = Mathf.Clamp((int)(100 - (game_score) * 10f) + 1, 0, 100);
        Debug.Log("Sanitization Game Score:");
        Debug.Log(game_score);

        main_controller.sanitization_score = game_score;
        main_controller.sanitization_done = true;

        base.EndScene();
    }
}