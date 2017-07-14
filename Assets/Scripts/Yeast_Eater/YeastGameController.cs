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
            float x_pos = Random.Range(yeast.position.x - 30f, yeast.position.x + 30f);
            float z_pos = Random.Range(yeast.position.z - 30f, yeast.position.z + 30f);

            GameObject sugar = Instantiate(sugar_prefab, new Vector3(x_pos, sugar_prefab.transform.position.y, z_pos), Quaternion.identity);
            //sugar.transform.Rotate(0f, Random.Range(0f, 360f), 0);
            Vector3 sugar_velocity = Random.onUnitSphere * 1f;
            sugar_velocity.y = 0f;
            sugar.GetComponent<Rigidbody>().velocity = sugar_velocity;
        }
    }
}
