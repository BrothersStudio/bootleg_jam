using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YeastGameController : GameControllers
{
    public Transform yeast;

    public int num_sugar = 100;
    public GameObject sugar_prefab;

    new void Start()
    {
        Cursor.visible = false;

        for (int ii = 0; ii < num_sugar; ii++)
        {
            GameObject sugar = Instantiate(sugar_prefab, GetRandVec3InCircle(5f, 40f), Quaternion.identity, transform);
            sugar.transform.Rotate(Random.Range(0f, 90f), Random.Range(0f, 90f), Random.Range(0f, 90f));
            Vector3 sugar_velocity = Random.onUnitSphere * 1f;
            sugar_velocity.y = 0f;
            sugar.GetComponent<Rigidbody>().velocity = sugar_velocity;
        }

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 20f);
        }

        base.Start();
    }

    void Update()
    {
        HandleTime(-(Time.timeSinceLevelLoad - 50f));
    }

    Vector3 GetRandVec3InCircle(float min_buffer, float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float dist = Random.Range(min_buffer, radius);

        Vector3 output = new Vector3();
        output.x = Mathf.Cos(angle) * dist;
        output.y = 0f;
        output.z = Mathf.Sin(angle) * dist;
        
        return output;
    }

    new void EndScene()
    {
        Cursor.visible = true;

        GameObject[] main_objects = SceneManager.GetSceneByName("Main").GetRootGameObjects();
        for (int i = 0; i < main_objects.Length; i++)
        {
            if (main_objects[i].name == "MainController")
            {
                main_objects[i].GetComponent<MainController>().yeast_done = true;
                main_objects[i].GetComponent<MainController>().RunNext();
            }
        }

        base.EndScene();
    }
}
