using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoilController : MonoBehaviour
{
	void Start ()
    {
        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 3f);
        }
    }

    void EndScene()
    {
        GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
        for (int i = 0; i < main_objects.Length; i++)
        {
            if (main_objects[i].name == "MainController")
            {
                main_objects[i].GetComponent<MainController>().boil_done = true;
                main_objects[i].GetComponent<MainController>().RunNext();
            }
        }
    }
}
