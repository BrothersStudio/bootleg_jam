using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodChecker : MonoBehaviour
{
    public PlayerTownController controller;

	void Update ()
    {
		if (controller.fed)
        {
            GetComponent<Text>().text = "Food obtained!";
            GetComponent<Text>().color = Color.green;
        }
	}
}
