using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisinfectantPool : MonoBehaviour
{
    public static DisinfectantPool current;

    public GameObject disinfectant_prefab;
    public int num_disinfectant;
    List<GameObject> disinfectant_pool;

    void Awake()
    {
        current = this;
    }

    void Start ()
    {
        disinfectant_pool = new List<GameObject>();
        for (int i = 0; i < num_disinfectant; i++)
        {
            GameObject obj = Instantiate(disinfectant_prefab, transform) as GameObject;
            obj.SetActive(false);
            disinfectant_pool.Add(obj);
        }
	}
	
    public GameObject GetPooledDisinfectant()
    {
        for (int i = 0; i < disinfectant_pool.Count; i++)
        {
            if (!disinfectant_pool[i].activeInHierarchy)
            {
                return disinfectant_pool[i];
            }
        }

        GameObject obj = Instantiate(disinfectant_prefab, transform) as GameObject;
        disinfectant_pool.Add(obj);
        return obj;
    }
}
