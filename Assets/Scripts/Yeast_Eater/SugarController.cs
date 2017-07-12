using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarController : MonoBehaviour
{
    public void Shrink()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(ShrinkCoroutine());
    }

    IEnumerator ShrinkCoroutine()
    {
        while (transform.localScale.x > 0.1)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(this.gameObject);
    }
}
