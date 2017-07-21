using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornCursor : MonoBehaviour
{
    bool scoop;

    string bug_tag = "Bug";

    public bool Scoop
    {
        get
        {
            return this.scoop;
        }

        set
        {
            this.scoop = value;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (scoop)
        {
            if (collision.gameObject.CompareTag(bug_tag))
            {
                collision.gameObject.SetActive(false);
            }
        }
    }
}
