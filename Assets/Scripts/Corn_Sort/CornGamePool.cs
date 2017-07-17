using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornGamePool : MonoBehaviour
{
    public static CornGamePool current;

    public GameObject corn_prefab;
    public GameObject bug_prefab;

    public int num_corn;
    public int num_bugs;

    public List<GameObject> corn_pool;
    public List<GameObject> bug_pool;

    void Awake()
    {
        current = this;
    }

    void Start ()
    {
        corn_pool = new List<GameObject>();
        for (int i = 0; i < num_corn; i++)
        {
            GameObject obj = Instantiate(corn_prefab, transform) as GameObject;
            obj.SetActive(false);
            corn_pool.Add(obj);
        }

        bug_pool = new List<GameObject>();
        for (int i = 0; i < num_bugs; i++)
        {
            GameObject obj = Instantiate(bug_prefab, transform) as GameObject;
            obj.SetActive(false);
            bug_pool.Add(obj);
        }
	}
	
    public GameObject GetPooledCorn()
    {
        for (int i = 0; i < corn_pool.Count; i++)
        {
            if (!corn_pool[i].activeInHierarchy)
            {
                return corn_pool[i];
            }
        }

        GameObject obj = Instantiate(corn_prefab, transform) as GameObject;
        corn_pool.Add(obj);
        return obj;
    }

    public GameObject GetPooledBug()
    {
        for (int i = 0; i < bug_pool.Count; i++)
        {
            if (!bug_pool[i].activeInHierarchy)
            {
                return bug_pool[i];
            }
        }

        GameObject obj = Instantiate(bug_prefab, transform) as GameObject;
        bug_pool.Add(obj);
        return obj;
    }
}
