using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanitizationController : MonoBehaviour
{
    public GameObject[] germ_patterns;

    float germ_separation = 80f;
    float ground_dist = 970f;

	void Start ()
    {
        Physics.gravity = new Vector3(0, -30.0F, 0);
        
        for (float i = germ_separation; i < ground_dist; i += germ_separation)
        {
            GameObject pattern = Instantiate(germ_patterns[Random.Range(0, germ_patterns.Length)]);
            Vector3 new_pos = pattern.transform.position;
            new_pos.x = i;
            pattern.transform.position = new_pos;
        }
    }
}
