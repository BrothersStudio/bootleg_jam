using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornCursor : MonoBehaviour
{
    bool scoop;

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
            Destroy(collision.gameObject);
        }
    }
}
