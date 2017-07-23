using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllers : MonoBehaviour
{
    protected GameObject timer;
    protected MainController main_controller = null;

    [HideInInspector]
    public bool started;
    public Text countdown_text;
    public GameObject countdown_screen;

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
        timer.SetActive(false);
    }

    protected IEnumerator StartCountdown(string display_text)
    {
        int i = 3;
        while (i > 0)
        {
            countdown_text.text = i.ToString();
            i--;
            yield return new WaitForSeconds(1f);
        }
        countdown_text.text = display_text;
        yield return new WaitForSeconds(0.7f);

        countdown_screen.SetActive(false);
        started = true;
        if (main_controller != null)
        {
            main_controller.main_time = 0f;
        }
    }

    protected void HandleTime(float current_time)
    {
        if (!timer.activeSelf)
        {
            timer.SetActive(true);
        }
        timer.GetComponent<Text>().text = string.Format("{0:0.00}", current_time);
    }

    protected void EndScene()
    {
        Destroy(timer);

        main_controller.RunNext();
    }
}
