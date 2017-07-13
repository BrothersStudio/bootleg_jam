using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    string faller_tag = "Faller";

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag(faller_tag))
            collision.gameObject.SetActive(false);
    }
}
