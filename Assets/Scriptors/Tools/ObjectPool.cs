using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool{

    private Queue<GameObject> pool;

    public ObjectPool()
    {
        pool = new Queue<GameObject>();
    }

    public void AddObject(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }

    public GameObject GetObject()
    {
        GameObject temp = null;
        if (pool.Count > 0)
        {
            temp = pool.Dequeue();
            temp.SetActive(true);
        }
        return temp;
    }

    public bool Data()
    {
        if (pool.Count > 0)
            return true;
        else
            return false;
    }
}
