using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoilController : GameControllers
{
    public int potential_score = 0;
    public int game_score = 0;

	new void Start ()
    {
        Cursor.visible = false;

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 10f);
        }

        base.Start();
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
        game_score = Mathf.Clamp((int)((float)game_score / (float)potential_score * 100f * 7f) + 1, 0, 100);
        Debug.Log("Boil Game Score:");
        Debug.Log(game_score);

        main_controller.boil_score = game_score;
        main_controller.boil_done = true;

        base.EndScene();
    }
}
