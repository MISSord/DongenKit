using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager 
{
    private static ObjectManager instance;
    //保存已经实例化的游戏物体
    private Dictionary<string, Queue<GameObject>> m_objectPool = new Dictionary<string, Queue<GameObject>>();
    //保存游戏资源
    private Dictionary<string, GameObject> m_ObjectPoolManager = new Dictionary<string, GameObject>();
    //对象池节点
    public Transform RecyclePoolTrs;
    //默认节点
    public Transform SceneTrs;

    private FactoryManager m_factoryManager;

    public static ObjectManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ObjectManager();
            }
            return instance;
        }
    }

    public void Init(Transform recycleTrs)
    {
        instance = this;
        RecyclePoolTrs = recycleTrs;
        m_objectPool.Clear();
        m_factoryManager = new FactoryManager();
        m_factoryManager.Init();
    }

    /// <summary>
    /// 获取游戏物体 通过提供工厂类型
    /// </summary>
    /// <param name="path"></param>
    /// <param name="factoryType"></param>
    /// <returns></returns>
    public GameObject GetGameObject(string path, FactoryType factoryType, bool IsParent)
    {
        GameObject _object = null;
        if (!m_objectPool.ContainsKey(path) || m_objectPool[path].Count == 0)
        {
            _object = GameObject.Instantiate(m_factoryManager.factoryDict[factoryType].GetGameObject(path));
            _object.name = path;
            if(!IsParent)
            {
                NewPoolToSave(path);
                _object.transform.SetParent(m_ObjectPoolManager[path].transform);
            }
        }
        else
        {
            _object = m_objectPool[path].Dequeue();
        }
        return _object;
    }

    /// <summary>
    /// 创建一件游戏物体池用于存储单个资源的多个游戏物体
    /// </summary>
    /// <param name="path"></param>
    public void NewPoolToSave(string path)
    {
        if (!m_objectPool.ContainsKey(path))
        {
            m_objectPool.Add(path, new Queue<GameObject>());
        }
        //创建一个对象
        if (!m_ObjectPoolManager.ContainsKey(path))
        {
            GameObject item = new GameObject(path + "Pool");
            item.transform.SetParent(RecyclePoolTrs);
            m_ObjectPoolManager.Add(path, item);
        }
    }

    public void PushObject(GameObject prefab)
    {
        string path = prefab.name;
        if (!m_objectPool.ContainsKey(path))
        {
            m_objectPool.Add(path, new Queue<GameObject>());
        }
        m_objectPool[path].Enqueue(prefab);
        GameObject m_item = null;
        if(m_ObjectPoolManager.TryGetValue(path, out m_item))
        {
            if (!prefab.transform.parent != m_item)
            {
                prefab.transform.SetParent(m_item.transform);
            }
        }
        prefab.SetActive(false);
    }



    protected Dictionary<Type, object> m_ClassPoolDic = new Dictionary<Type, object>();

    public ClassObjectPool<T> GetOrCreatClassPool<T>(int maxcount) where T : class, new()
    {
        System.Type type = typeof(T);
        object outObj = null;
        if (!m_ClassPoolDic.TryGetValue(type, out outObj) || outObj == null)
        {
            ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxcount);
            m_ClassPoolDic.Add(type, newPool);
            return newPool;
        }

        return outObj as ClassObjectPool<T>;
    }
}
