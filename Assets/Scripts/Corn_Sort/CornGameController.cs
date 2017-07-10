using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornGameController : MonoBehaviour
{
    public GameObject corn_cursor;
    public GameObject corn_prefab;
    public GameObject bug_prefab;

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
    }

    void Update()
    {
        // Move circle cursor
        Camera cam = Camera.main;
        Vector3 mouse_pos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = 0;
        corn_cursor.transform.position = mouse_pos;

        // Spawn things
        for (float i = 0; i < (int)((Time.timeSinceLevelLoad - last_spawn) * fallers_per_second); i++)
        {
            if (Random.Range(0f, 100f) <= bug_spawn_percent)
            {
                // I don't want the bugs to spawn too close to the edge
                float spawn_x = Random.Range(-screen_width_pos + 2f, screen_width_pos - 2f);
                GameObject thing = Instantiate(bug_prefab, new Vector2(spawn_x, screen_top_pos), Quaternion.identity);
                thing.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
            }
            else
            {
                float spawn_x = Random.Range(-screen_width_pos, screen_width_pos);
                GameObject thing = Instantiate(corn_prefab, new Vector2(spawn_x, screen_top_pos), Quaternion.identity);
                thing.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
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
