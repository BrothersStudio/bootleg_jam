using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionGoal : MonoBehaviour
{
    SanitizationController controller;

    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<SanitizationController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Infection")
            controller.game_score++;
    }
}
