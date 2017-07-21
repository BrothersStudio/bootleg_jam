using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoilController : GameControllers
{
    int game_score = 0;

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
        Debug.Log(Time.timeSinceLevelLoad);
        HandleTime(-(Time.timeSinceLevelLoad - 60f));
    }

    new void EndScene()
    {
        Cursor.visible = true;

        GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
        for (int i = 0; i < main_objects.Length; i++)
        {
            if (main_objects[i].name == "MainController")
            {
                main_objects[i].GetComponent<MainController>().score += game_score;
                main_objects[i].GetComponent<MainController>().boil_done = true;
                main_objects[i].GetComponent<MainController>().RunNext();
            }
        }

        base.EndScene();
    }
}
