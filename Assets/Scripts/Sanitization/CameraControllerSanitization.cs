using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerSanitization : MonoBehaviour
{
    public GameObject to_track;

    Vector3 offset;
    float original_y;

    void Start()
    {
        offset = transform.position - to_track.transform.position;
        original_y = transform.position.y;
    }

    void LateUpdate()
    {
        Vector3 new_position = to_track.transform.position + offset;
        new_position.y = original_y;
        transform.position = new_position;
    }
}
