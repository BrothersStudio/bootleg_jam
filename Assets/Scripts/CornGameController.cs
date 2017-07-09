using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornGameController : MonoBehaviour
{
    public GameObject corn_cursor;
    public GameObject corn_prefab;
    public GameObject bug_prefab;

    public float click_circle_radius;

    public float bug_spawn_percent;

    public float spawn_rate;
    float last_spawn = 0f;

    float screen_width_pos;
    float screen_top_pos;

    void Start()
    {
        Vector3 far_corner = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f));
        screen_width_pos = -far_corner.x;
        screen_top_pos = -far_corner.y;
    }

    void Update ()
    {
        // Move circle cursor
        Camera cam = Camera.main;
        Vector3 mouse_pos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = 0;
        corn_cursor.transform.position = mouse_pos;

        // Spawn things
        if (Time.timeSinceLevelLoad > last_spawn)
        {
            last_spawn = Time.timeSinceLevelLoad + spawn_rate;

            float spawn_x = Random.Range(-screen_width_pos, screen_width_pos);
            if (Random.Range(0f, 100f) <= bug_spawn_percent)
            {
                GameObject bug = Instantiate(bug_prefab, new Vector2(spawn_x, screen_top_pos), Quaternion.identity);
                bug.GetComponent<Faller>().Bottom = -screen_top_pos;
            }
            else
            {
                GameObject corn = Instantiate(corn_prefab, new Vector2(spawn_x, screen_top_pos), Quaternion.identity);
                corn.GetComponent<Faller>().Bottom = -screen_top_pos;
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
