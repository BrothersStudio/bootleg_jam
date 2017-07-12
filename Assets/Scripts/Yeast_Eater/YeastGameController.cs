using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeastGameController : MonoBehaviour {

    public int num_sugar = 1000;
    public GameObject sugar_prefab;

    void Start()
    {
        for (int ii = 0; ii < num_sugar; ii++)
        {
            float x_pos = Random.Range(-600f, 600f);
            float y_pos = Random.Range(-340f, 340f);

            GameObject sugar = Instantiate(sugar_prefab, new Vector3(x_pos, y_pos, 0f), Quaternion.identity);
            sugar.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
            sugar.GetComponent<Rigidbody2D>().velocity = Random.onUnitSphere * 1f;
        }
    }
}
