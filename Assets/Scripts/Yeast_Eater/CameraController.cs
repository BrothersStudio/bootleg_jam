using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject yeast;

    Vector3 offset;

	void Start ()
    {
        offset = transform.position - yeast.transform.position;
    }
	
	void LateUpdate ()
    {
        transform.position = yeast.transform.position + offset;
    }
}
