using System.Collections;
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
    public LayerMask teal_mask;
    public LayerMask yellow_mask;

    public GameObject teal_vial;
    public GameObject yellow_vial;

    int current_selection = -1;
    public AudioClip color_select_clip;
    AudioSource source;

    public float spray_speed;
    public float spray_cooldown;
    float next_spray = 0f;
    public AudioSource spray_source;

    int low_infections_per_kettle;
    int high_infections_per_kettle;
    public float kettle_spawn_period;
    float next_kettle = 0f;

    int total_infections = 0;
    public int game_score = 0;
    public int difficulty = 1;
    int infection_options = 3;

    public GameObject upgrade_tutorial_text;
    public bool sanitization_upgrade = false;
    [HideInInspector]
    public bool freeze = false;
    bool freeze_used = false;
    public AudioClip freeze_tick_clip;

    new void Start()
    {
        Cursor.visible = false;
        source = GetComponent<AudioSource>();

        base.Start();

        if (main_controller != null)
        {
            difficulty = main_controller.current_difficulty;
            sanitization_upgrade = main_controller.sanitization_upgrade;
        }

        SetUpgrades();
        SetDifficulty();
        StartCoroutine(StartCountdown("Sanitize!"));

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 23.7f);
        }
    }

    void SetUpgrades()
    {
        if (sanitization_upgrade)
        {
            upgrade_tutorial_text.SetActive(true);
        }
    }

    void SetDifficulty()
    {
        if (difficulty == 10)
        {
            infection_options = 5;
            low_infections_per_kettle = 3;
            high_infections_per_kettle = 4;

            kettle_spawn_period = 2.2f;
        }
        else if (difficulty == 9)
        {
            infection_options = 5;
            low_infections_per_kettle = 2;
            high_infections_per_kettle = 4;

            kettle_spawn_period = 2.3f;
        }
        else if (difficulty == 8)
        {
            infection_options = 5;
            low_infections_per_kettle = 3;
            high_infections_per_kettle = 3;

            kettle_spawn_period = 2.4f;
        }
        else if (difficulty == 7)
        {
            infection_options = 5;
            low_infections_per_kettle = 2;
            high_infections_per_kettle = 3;

            kettle_spawn_period = 2.5f;
        }
        else if (difficulty == 6)
        {
            infection_options = 4;
            low_infections_per_kettle = 2;
            high_infections_per_kettle = 4;

            kettle_spawn_period = 2.5f;
        }
        else if (difficulty == 5)
        {
            infection_options = 4;
            low_infections_per_kettle = 2;
            high_infections_per_kettle = 3;

            kettle_spawn_period = 2.5f;
        }
        else if (difficulty == 4)
        {
            infection_options = 4;
            low_infections_per_kettle = 2;
            high_infections_per_kettle = 3;

            kettle_spawn_period = 2.5f;
        }
        else if (difficulty == 3)
        {
            infection_options = 3;
            low_infections_per_kettle = 1;
            high_infections_per_kettle = 3;

            kettle_spawn_period = 3f;
        }
        else if (difficulty == 2)
        {
            infection_options = 3;
            low_infections_per_kettle = 1;
            high_infections_per_kettle = 2;

            kettle_spawn_period = 3f;
        }
        else if (difficulty == 1)
        {
            infection_options = 3;
            low_infections_per_kettle = 1;
            high_infections_per_kettle = 1;

            kettle_spawn_period = 3f;
        }

        if (infection_options == 4)
        {
            teal_vial.SetActive(true);
        }
        else if (infection_options == 5)
        {
            teal_vial.SetActive(true);
            yellow_vial.SetActive(true);
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

            if (sanitization_upgrade && !freeze_used && Input.GetKeyDown("space"))
            {
                freeze = true;
                freeze_used = true;
                next_kettle += 2f;
                StartCoroutine(TimeStop());
            }

            HandleDisinfectant();

            SpawnKettles();
        }
    }

    IEnumerator TimeStop()
    {
        for (int i = 0; i < 3; i++)
        {
            source.clip = freeze_tick_clip;
            source.Play();

            yield return new WaitForSeconds(1f);
        }
        source.clip = start_clip;
        source.Play();
        freeze = false;
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
                if (!source.isPlaying)
                {
                    source.clip = color_select_clip;
                    source.Play();
                }
                current_selection = 0;
                return;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, green_mask))
            {
                if (!source.isPlaying)
                {
                    source.clip = color_select_clip;
                    source.Play();
                }
                current_selection = 1;
                return;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, blue_mask))
            {
                if (!source.isPlaying)
                {
                    source.clip = color_select_clip;
                    source.Play();
                }
                current_selection = 2;
                return;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, teal_mask))
            {
                if (!source.isPlaying)
                {
                    source.clip = color_select_clip;
                    source.Play();
                }
                current_selection = 3;
                return;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, yellow_mask))
            {
                if (!source.isPlaying)
                {
                    source.clip = color_select_clip;
                    source.Play();
                }
                current_selection = 4;
                return;
            }

            if (Time.timeSinceLevelLoad > next_spray && current_selection >= 0)
            {
                next_spray = Time.timeSinceLevelLoad + spray_cooldown;
                spray_source.Play();

                for (int i = 0; i < 30; i++)
                {
                    GameObject disinfectant = DisinfectantPool.current.GetPooledDisinfectant();
                    Vector3 new_pos = sanitization_spawn_loc.transform.position;
                    new_pos.x += Random.Range(-2f, 2f);
                    new_pos.y += Random.Range(-2f, 2f);
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
                        case 3:
                            disinfectant.tag = "Teal";
                            break;
                        case 4:
                            disinfectant.tag = "Yellow";
                            break;
                    }
                }
            }
        }
    }

    void SpawnKettles()
    {
        if (Time.timeSinceLevelLoad > next_kettle && !freeze)
        {
            next_kettle = Time.timeSinceLevelLoad + kettle_spawn_period;

            GameObject kettle = Instantiate(kettle_prefab, new Vector3(-22f, 25f, 79.1f), Quaternion.identity, transform);
            kettle.transform.Rotate(new Vector3(-90f, 0f, 0f));

            int infection_num = Random.Range(low_infections_per_kettle, high_infections_per_kettle + 1);
            for (int i = 0; i < infection_num; i++)
            {
                total_infections++;

                GameObject infection = Instantiate(infection_prefab, kettle.transform);
                infection.name = "Infection";
                infection.transform.localPosition = new Vector3(Random.Range(-1f, 1f), 1.62f, Random.Range(-0.5f, 1.5f));

                int infection_roll = Random.Range(0, infection_options);
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
                    case 3:
                        infection.tag = "Teal";
                        break;
                    case 4:
                        infection.tag = "Yellow";
                        break;
                }
            }
        }
    }

    new void EndScene()
    {
        Cursor.visible = true;
        game_score = Mathf.Clamp((100 - game_score * 10), 0, 100);
        Debug.Log("Sanitization Game Score:");
        Debug.Log(game_score);

        main_controller.sanitization_score = game_score;
        main_controller.sanitization_done = true;

        base.EndScene();
    }
}