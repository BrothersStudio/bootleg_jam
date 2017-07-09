using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faller : MonoBehaviour
{
    float bottom;

    public float Bottom
    {
        get
        {
            return this.bottom;
        }

        set
        {
            this.bottom = value;
        }
    }

	void Update ()
    {
        if (transform.position.y < bottom)
            Destroy(this.gameObject);
    }
}
