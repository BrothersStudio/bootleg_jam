using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoilController : GameControllers
{
    List<bool> boil_upgrades;

    public ZoneMovement good_zone;

    public int potential_score = 0;
    public int game_score = 0;
    public int difficulty = 1;

	new void Start ()
    {
        Cursor.visible = false;
        
        base.Start();

        if (main_controller != null)
        {
            difficulty = main_controller.current_difficulty;
            boil_upgrades = main_controller.boil_upgrades;
        }
        else
        {
            boil_upgrades = new List<bool>(new bool[] { false });
        }

        SetUpgrades();
        SetDifficulty();
        StartCoroutine(StartCountdown("Boil!"));

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 13.7f);
        }
    }

    void SetUpgrades()
    {
        if (boil_upgrades[0])
        {
            Physics.gravity = new Vector3(0, -20F, 0);
        }
        else
        {
            Physics.gravity = new Vector3(0, -15F, 0);
        }
    }

    void SetDifficulty()
    {
        if (difficulty > 6)
        {
            good_zone.zone_speed = 2.2f;
            good_zone.flip_cooldown = 2.5f;
        }
        else if (difficulty > 3)
        {
            good_zone.zone_speed = 2f;
            good_zone.flip_cooldown = 3f;
        }
        else 
        {
            good_zone.zone_speed = 1.7f;
            good_zone.flip_cooldown = 3.5f;
        }
    }

    void Update()
    {
        if (started)
        {
            if (main_controller != null)
            {
                HandleTime(-(main_controller.main_time - 10f));
            }

            potential_score++;
        }
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
