using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornCursor : MonoBehaviour
{
    public Camera corn_camera;

    string corn_tag = "Corn";
    string bug_tag = "Bug";

    CornGameController controller;
    AudioSource cursor_audio;
    public AudioClip good_sound;
    public AudioClip bad_sound;

    void Start()
    {
        controller = GameObject.Find("CornGameController").GetComponent<CornGameController>();
        cursor_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Move circle cursor
        Vector3 mouse_pos = corn_camera.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = transform.position.z;
        transform.position = mouse_pos;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag(corn_tag))
        {
            controller.hit_corn++;
            collision.gameObject.SetActive(false);
            cursor_audio.clip = bad_sound;
            cursor_audio.Play();
        }
        else if (collision.gameObject.CompareTag(bug_tag))
        {
            collision.gameObject.SetActive(false);
            cursor_audio.clip = good_sound;
            cursor_audio.Play();
        }
    }
}
