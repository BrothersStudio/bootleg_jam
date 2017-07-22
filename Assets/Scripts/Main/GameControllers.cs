using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllers : MonoBehaviour
{
    protected GameObject timer;
    protected MainController main_controller = null;

    protected void Start()
    {
        if (SceneManager.sceneCount > 1)
        {
            GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
            for (int i = 0; i < main_objects.Length; i++)
            {
                if (main_objects[i].name == "MainController")
                {
                    main_controller = main_objects[i].GetComponent<MainController>();
                    break;
                }
            }
        }

        GameObject timer_prefab = Resources.Load("Prefabs\\Timer") as GameObject;
        GameObject canvas = GameObject.Find("Canvas");
        timer = Instantiate(timer_prefab, canvas.transform);
    }

    protected void HandleTime(float current_time)
    {
        timer.GetComponent<Text>().text = string.Format("{0:0.00}", current_time);
    }

    protected void EndScene()
    {
        Destroy(timer);

        main_controller.RunNext();
    }
}
