using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SanitizationController : MonoBehaviour
{
    public GameObject[] germ_patterns;

    float germ_separation = 80f;
    float ground_dist = 970f;

	void Start()
    {
        Physics.gravity = new Vector3(0, -30.0F, 0);
        
        for (float i = germ_separation; i < ground_dist; i += germ_separation)
        {
            GameObject pattern = Instantiate(germ_patterns[Random.Range(0, germ_patterns.Length)], transform);
            Vector3 new_pos = pattern.transform.position;
            new_pos.x = i;
            pattern.transform.position = new_pos;
        }

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
                main_objects[i].GetComponent<MainController>().sanitization_done = true;
                main_objects[i].GetComponent<MainController>().RunNext();
            }
        }
    }
}
