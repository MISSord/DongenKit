using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameRoot gameRoot;
    private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    [HideInInspector]
    public GameObject pool;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    public void Init()
    {
        gameRoot = GameRoot.Instance;
        pool = new GameObject();
        pool.AddComponent<DontDestroyOnLoad>();
        //pool.transform.SetParent(gameRoot.transform);
        objectPool.Clear();
    }

    public GameObject GetObject(string name)
    {
        GameObject _object;
        if (!objectPool.ContainsKey(name) || objectPool[name].Count == 0)
        {
            _object = gameRoot.GetGameOb(name);
            _object.name = name;
            PushObject(_object);
            if (pool == null)
                pool = new GameObject("ObjectPool");
            GameObject childPool = GameObject.Find(name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(childPool.transform);
        }
        _object = objectPool[name].Dequeue();
        _object.SetActive(true);
        return _object;
    }

    public void PushObject(GameObject prefab)
    {
        string name = prefab.name;
        if (!objectPool.ContainsKey(name))
            objectPool.Add(name, new Queue<GameObject>());
        objectPool[name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}
