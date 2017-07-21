using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CornGameController : GameControllers
{
    public GameObject corn_cursor;
    public Camera corn_camera;

    Vector3 spawn_loc;
    Vector3 spawn_vel;
    public float bug_spawn_percent;

    public float faller_cooldown = 0.1f;
    float next_spawn = 0f;

    float screen_width_pos;
    float screen_top_pos;

    new void Start()
    {
        Vector3 far_corner = corn_camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        screen_width_pos = -far_corner.x;
        screen_top_pos = -far_corner.y;

        spawn_loc = new Vector3(0f, 0f, 0f);
        spawn_vel = new Vector3(0f, 0f, 0f);

        Physics.gravity = new Vector3(0, -4.0F, 0);

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 10f);
        }

        base.Start();
    }

    void Update()
    {
        HandleTime(-(Time.timeSinceLevelLoad - 30f));

        // Move circle cursor
        Vector3 mouse_pos = corn_camera.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = corn_cursor.transform.position.z;
        corn_cursor.transform.position = mouse_pos;

        // Spawn things
        if (Time.timeSinceLevelLoad > next_spawn)
        {
            next_spawn = Time.timeSinceLevelLoad + faller_cooldown;

            if (Random.Range(0f, 100f) <= bug_spawn_percent)
            {
                GameObject bug = CornGamePool.current.GetPooledBug();

                // I don't want the bugs to spawn too close to the edge
                spawn_loc.x = Random.Range(-screen_width_pos + 2f, screen_width_pos - 2f);
                spawn_loc.y = screen_top_pos + 8f;
                spawn_loc.z = -2f;
                bug.transform.position = spawn_loc;
                bug.GetComponent<Rigidbody>().velocity = spawn_vel;
                bug.transform.Rotate(Random.Range(0f, 360f), 0f, 0f);
                bug.SetActive(true);
            }
            else
            {
                GameObject corn = CornGamePool.current.GetPooledCorn();

                spawn_loc.x = Random.Range(-screen_width_pos, screen_width_pos);
                spawn_loc.y = screen_top_pos + 4f;
                spawn_loc.z = 2f;
                corn.transform.position = spawn_loc;
                corn.GetComponent<Rigidbody>().velocity = spawn_vel;
                corn.transform.Rotate(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                corn.SetActive(true);
            }
        }

        // Scoop things
        if (Input.GetMouseButton(0))
        {
            corn_cursor.GetComponent<CornCursor>().Scoop = true;
        }
        else
        {
            corn_cursor.GetComponent<CornCursor>().Scoop = false;
        }
    }

    new void EndScene()
    {
        GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
        for (int i = 0; i < main_objects.Length; i++)
        {
            if (main_objects[i].name == "MainController")
            {
                main_objects[i].GetComponent<MainController>().corn_done = true;
                main_objects[i].GetComponent<MainController>().RunNext();
            }
        }

        base.EndScene();
    }
}
