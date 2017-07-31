using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YeastGameController : GameControllers
{
    public GameObject yeast;
    public GameObject bad_yeast_prefab;

    public int num_sugar = 100;
    public GameObject sugar_prefab;

    public int game_score;
    public int difficulty;
    public bool yeast_upgrade = false;

    new void Start()
    {
        for (int ii = 0; ii < num_sugar; ii++)
        {
            GameObject sugar = Instantiate(sugar_prefab, GetRandVec3InCircle(5f, 40f, 1f), Quaternion.identity, transform);
            sugar.transform.Rotate(Random.Range(0f, 90f), Random.Range(0f, 90f), Random.Range(0f, 90f));
            Vector3 sugar_velocity = Random.onUnitSphere * 1f;
            sugar_velocity.y = 0f;
            sugar.GetComponent<Rigidbody>().velocity = sugar_velocity;
        }

        base.Start();

        if (main_controller != null)
        {
            difficulty = main_controller.current_difficulty;
            yeast_upgrade = main_controller.yeast_upgrade;
        }

        SetUpgrades();
        SetDifficulty();
        StartCoroutine(StartCountdown("Eat!"));

        if (SceneManager.sceneCount > 1)
        {
            Invoke("EndScene", 23.7f);
        }
    }

    void SetUpgrades()
    {
        if (yeast_upgrade)
        {
            yeast.GetComponent<YeastController>().yeast_speed = 350f;
            yeast.GetComponent<Rigidbody>().drag = 1.5f;
        }
    }

    void SetDifficulty()
    {
        if (difficulty == 10)
        {
            GameObject bad_yeast1 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast1.GetComponent<BadYeastController>().enemy_speed = 70f;
            GameObject bad_yeast2 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast2.GetComponent<BadYeastController>().enemy_speed = 70f;
        }
        else if (difficulty == 9)
        {
            GameObject bad_yeast1 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast1.GetComponent<BadYeastController>().enemy_speed = 55f;
            GameObject bad_yeast2 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast2.GetComponent<BadYeastController>().enemy_speed = 55f;
        }
        else if (difficulty == 8)
        {
            GameObject bad_yeast1 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast1.GetComponent<BadYeastController>().enemy_speed = 50f;
            GameObject bad_yeast2 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast2.GetComponent<BadYeastController>().enemy_speed = 50f;
        }
        else if (difficulty == 7)
        {
            GameObject bad_yeast1 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast1.GetComponent<BadYeastController>().enemy_speed = 45f;
            GameObject bad_yeast2 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast2.GetComponent<BadYeastController>().enemy_speed = 45f;
        }
        else if (difficulty == 6)
        {
            GameObject bad_yeast1 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast1.GetComponent<BadYeastController>().enemy_speed = 40f;
            GameObject bad_yeast2 = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast2.GetComponent<BadYeastController>().enemy_speed = 40f;
        }
        else if (difficulty == 5)
        {
            GameObject bad_yeast = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(15f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast.GetComponent<BadYeastController>().enemy_speed = 60f;
        }
        else if (difficulty == 4)
        {
            GameObject bad_yeast = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(20f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast.GetComponent<BadYeastController>().enemy_speed = 50f;
        }
        else if (difficulty == 3)
        {
            GameObject bad_yeast = Instantiate(bad_yeast_prefab, GetRandVec3InCircle(25f, 40f, 0f), Quaternion.identity, transform);
            bad_yeast.GetComponent<BadYeastController>().enemy_speed = 40f;
        }
    }

    void Update()
    {
        if (started)
        {
            if (main_controller != null)
            {
                HandleTime(-(main_controller.main_time - 20f));
            }
        }
    }

    Vector3 GetRandVec3InCircle(float min_buffer, float radius, float height)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float dist = Random.Range(min_buffer, radius);

        Vector3 output = new Vector3();
        output.x = Mathf.Cos(angle) * dist;
        output.y = height;
        output.z = Mathf.Sin(angle) * dist;
        
        return output;
    }

    new void EndScene()
    {
        game_score = Mathf.Clamp((int)(game_score * 3.7f * (100f / (float)num_sugar)) + 1, 0, 100);
        Debug.Log("Yeast Game Score:");
        Debug.Log(game_score);

        main_controller.yeast_score = game_score;
        main_controller.yeast_done = true;

        base.EndScene();
    }
}
