using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllers : MonoBehaviour
{
    protected GameObject timer;

    protected void Start()
    {
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
    }
}
