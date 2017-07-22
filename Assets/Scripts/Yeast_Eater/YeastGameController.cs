using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YeastGameController : GameControllers
{
    public Transform yeast;

    public int num_sugar = 100;
    public GameObject sugar_prefab;

    public int game_score;

    new void Start()
    {
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
        if (main_controller != null)
        {
            HandleTime(-(main_controller.main_time - 20f));
        }
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
        game_score = Mathf.Clamp((int)(game_score * 4f * (100f / (float)num_sugar)) + 1, 0, 100);
        Debug.Log("Yeast Game Score:");
        Debug.Log(game_score);

        main_controller.yeast_score = game_score;
        main_controller.yeast_done = true;

        base.EndScene();
    }
}
