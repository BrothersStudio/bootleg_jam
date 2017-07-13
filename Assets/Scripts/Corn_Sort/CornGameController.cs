using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornGameController : MonoBehaviour
{
    public GameObject corn_cursor;

    Vector3 spawn_loc;
    Vector3 spawn_vel;
    public float bug_spawn_percent;

    public float fallers_per_second;
    float last_spawn = 0f;

    float screen_width_pos;
    float screen_top_pos;

    void Start()
    {
        Vector3 far_corner = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f));
        screen_width_pos = -far_corner.x;
        screen_top_pos = -far_corner.y;

        spawn_loc = new Vector3(0f, 0f, 0f);
        spawn_vel = new Vector3(0f, 0f, 0f);
    }

    void Update()
    {
        // Move circle cursor
        Camera cam = Camera.main;
        Vector3 mouse_pos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = corn_cursor.transform.position.z;
        corn_cursor.transform.position = mouse_pos;

        // Spawn things
        for (float i = 0; i < (int)((Time.timeSinceLevelLoad - last_spawn) * fallers_per_second) + 1; ++i)
        {
            if (Random.Range(0f, 100f) <= bug_spawn_percent)
            {
                GameObject bug = CornGamePool.current.GetPooledBug();

                // I don't want the bugs to spawn too close to the edge
                spawn_loc.x = Random.Range(-screen_width_pos + 2f, screen_width_pos - 2f);
                spawn_loc.y = screen_top_pos + 8f;
                spawn_loc.z = -2f;
                bug.transform.position = spawn_loc;
                bug.GetComponent<Rigidbody>().velocity = spawn_vel;
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
        last_spawn = Time.timeSinceLevelLoad;

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
