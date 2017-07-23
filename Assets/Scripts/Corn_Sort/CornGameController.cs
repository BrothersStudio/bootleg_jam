using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CornGameController : GameControllers
{
    public GameObject corn_cursor;
    public Camera corn_camera;
    public ParticleSystem crumbs;

    Vector3 spawn_loc;
    Vector3 spawn_vel;
    public float bug_spawn_percent;

    public float faller_cooldown = 0.1f;
    float next_spawn = 0f;

    float screen_width_pos;
    float screen_top_pos;

    int total_bugs = 0;
    public int game_score = 0;
    public int difficulty = 1;

    new void Start()
    {
        Vector3 far_corner = corn_camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        screen_width_pos = -far_corner.x;
        screen_top_pos = -far_corner.y;

        spawn_loc = new Vector3(0f, 0f, 0f);
        spawn_vel = new Vector3(0f, 0f, 0f);

        base.Start();

        if (main_controller != null)
        {
            difficulty = main_controller.current_difficulty;
        }

        SetDifficulty();
        StartCoroutine(StartCountdown("Remove!"));

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 13.7f);
        }
    }

    void SetDifficulty()
    {
        float fall_speed = -4f;

        if (difficulty >= 7)
        {
            fall_speed = -6f;

            bug_spawn_percent = difficulty * 3 + 5;
        }
        else if (difficulty >= 4)
        {
            fall_speed = -5f;

            bug_spawn_percent = difficulty * 2 + 5;
        }
        else // (difficulty < 4)
        {
            fall_speed = -4f;

            bug_spawn_percent = difficulty + 5;
        }

        Physics.gravity = new Vector3(0, fall_speed, 0);
    }

    void Update()
    {
        // Move circle cursor
        Vector3 mouse_pos = corn_camera.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = corn_cursor.transform.position.z;
        corn_cursor.transform.position = mouse_pos;

        if (started)
        {
            if (main_controller != null)
            {
                HandleTime(-(main_controller.main_time - 10f));
            }

            // Spawn things
            if (Time.timeSinceLevelLoad > next_spawn)
            {
                next_spawn = Time.timeSinceLevelLoad + faller_cooldown;

                // Turn on particle crumbs
                if (!crumbs.gameObject.activeSelf)
                {
                    crumbs.gameObject.SetActive(true);
                }

                if (Random.Range(0f, 100f) <= bug_spawn_percent)
                {
                    total_bugs++;
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
    }

    new void EndScene()
    {
        game_score = Mathf.Clamp((int)((total_bugs - game_score) / (float)total_bugs * 100f) + 1, 0, 100);
        Debug.Log("Corn Game Score:");
        Debug.Log(game_score);

        main_controller.corn_score = game_score;
        main_controller.corn_done = true; 

        base.EndScene();
    }
}
