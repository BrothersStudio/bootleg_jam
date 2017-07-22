using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoilController : GameControllers
{
    public ZoneMovement good_zone;

    public int potential_score = 0;
    public int game_score = 0;
    public int difficulty = 1;

	new void Start ()
    {
        Cursor.visible = false;
        Physics.gravity = new Vector3(0, -15.0F, 0);

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 10f);
        }

        base.Start();

        if (main_controller != null)
        {
            difficulty = main_controller.current_difficulty;
        }

        SetDifficulty();
    }

    void SetDifficulty()
    {
        if (difficulty > 6)
        {
            good_zone.zone_speed = 2.2f;
            good_zone.flip_cooldown = 3f;
        }
        else if (difficulty > 3)
        {
            good_zone.zone_speed = 2f;
            good_zone.flip_cooldown = 3.5f;
        }
        else 
        {
            good_zone.zone_speed = 1.7f;
            good_zone.flip_cooldown = 4f;
        }
    }

    void Update()
    {
        if (main_controller != null)
        {
            HandleTime(-(main_controller.main_time - 10f));
        }

        potential_score++;
    }

    new void EndScene()
    {
        Cursor.visible = true;
        Physics.gravity = new Vector3(0, -9.8F, 0);

        game_score = Mathf.Clamp((int)((float)game_score / (float)potential_score * 100f * 7f) + 1, 0, 100);
        Debug.Log("Boil Game Score:");
        Debug.Log(game_score);

        main_controller.boil_score = game_score;
        main_controller.boil_done = true;

        base.EndScene();
    }
}
