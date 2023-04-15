using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject objectToPool;
    public int poolSize;
    public bool canGrow;

    private List<GameObject> pool;

    private void Awake()
    {
        Instance = this;

        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectToPool, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject(string name)
    {
        foreach (GameObject obj in pool)
        {
            if (obj.name == name && !obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        if (canGrow)
        {
            GameObject obj = Instantiate(objectToPool, transform);
            obj.SetActive(true);
            pool.Add(obj);
            return obj;
        }

        return null;
    }

    public void ReturnObject(string name)
    {
        foreach (GameObject obj in pool)
        {
            if (obj.name == name && obj.activeSelf)
            {
                obj.SetActive(false);
                return;
            }
        }
    }
}
