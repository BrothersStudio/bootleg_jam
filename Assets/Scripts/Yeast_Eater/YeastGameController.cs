using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeastGameController : MonoBehaviour
{
    public Transform yeast;

    public int num_sugar = 100;
    public GameObject sugar_prefab;

    void Start()
    {
        for (int ii = 0; ii < num_sugar; ii++)
        {
            GameObject sugar = Instantiate(sugar_prefab, GetRandVec3InCircle(5f, 40f), Quaternion.identity);
            sugar.transform.Rotate(Random.Range(0f, 90f), Random.Range(0f, 90f), Random.Range(0f, 90f));
            Vector3 sugar_velocity = Random.onUnitSphere * 1f;
            sugar_velocity.y = 0f;
            sugar.GetComponent<Rigidbody>().velocity = sugar_velocity;
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
}
