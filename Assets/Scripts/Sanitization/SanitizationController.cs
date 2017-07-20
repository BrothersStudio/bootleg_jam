using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SanitizationController : MonoBehaviour
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

    public int current_selection = -1;

    public float spray_speed;
    public float spray_cooldown;
    float next_spray = 0f;

    public float kettle_spawn_period;
    float next_kettle = 0f;

    void Start()
    {
        Cursor.visible = false;

        if (SceneManager.sceneCount > 1)
        {
            //Invoke("EndScene", 10f);
        }
    }

    void Update()
    {
        HandleDisinfectant();

        SpawnKettles();
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
                current_selection = 0;
                return;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, green_mask))
            {
                current_selection = 1;
                return;
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, blue_mask))
            {
                current_selection = 2;
                return;
            }

            if (Time.timeSinceLevelLoad > next_spray && current_selection >= 0)
            {
                next_spray = Time.timeSinceLevelLoad + spray_cooldown;

                GameObject disinfectannt = DisinfectantPool.current.GetPooledDisinfectant();
                disinfectannt.transform.position = sanitization_spawn_loc.transform.position;

                disinfectannt.GetComponent<Rigidbody>().velocity = sanitization_spawn_loc.transform.forward * spray_speed;

                disinfectannt.GetComponent<MeshRenderer>().material = color_mats[current_selection];
                disinfectannt.SetActive(true);

                switch (current_selection)
                {
                    case 0:
                        disinfectannt.tag = "Red";
                        break;
                    case 1:
                        disinfectannt.tag = "Green";
                        break;
                    case 2:
                        disinfectannt.tag = "Blue";
                        break;
                }
            }
        }
    }

    void SpawnKettles()
    {
        if (Time.timeSinceLevelLoad > next_kettle)
        {
            next_kettle = Time.timeSinceLevelLoad + kettle_spawn_period;

            GameObject kettle = Instantiate(kettle_prefab, new Vector3(-20f, 0f, -2.2f), Quaternion.identity, transform);
            kettle.transform.Rotate(new Vector3(-90f, 0f, 0f));

            GameObject infection = Instantiate(infection_prefab, kettle.transform);
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

    void EndScene()
    {
        Cursor.visible = true;

        GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
        for (int i = 0; i < main_objects.Length; i++)
        {
            if (main_objects[i].name == "MainController")
            {
                main_objects[i].GetComponent<MainController>().sanitization_done = true;
                main_objects[i].GetComponent<MainController>().RunNext();
            }
        }
    }
}
