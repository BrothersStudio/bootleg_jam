using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject to_track;

    Vector3 offset;

	void Start ()
    {
        offset = transform.position - to_track.transform.position;
    }
	
	void LateUpdate ()
    {
        transform.position = to_track.transform.position + offset;
    }
}
