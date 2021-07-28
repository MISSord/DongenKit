using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AssetServer : MonoBehaviour
{
    public static AssetServer Instance = null;

    public ObjectManager m_ObjectPool;
    private ResourceManager m_ResourceManager;
    private Transform m_ObjectPoolTran;

    public void Init()
    {
        Instance = this;
        m_ObjectPoolTran = transform.Find("ObjectPool");

        m_ResourceManager = new ResourceManager();
        m_ResourceManager.Init();

        m_ObjectPool = ObjectManager.Instance;
        m_ObjectPool.Init(m_ObjectPoolTran);


        MessageServer.AddListener<GameObject>(EventType.PushToPool, PushObjectToPool);

        //简单操作
        MessageServer.AddListener<string, Action<GameObject>>(EventType.GetAndSetGameObject, GetAndSetGameObject);

        MessageServer.AddListener<string,bool>(EventType.PlayMusicOrBG, GetAudioClip);
        MessageServer.AddListener<string>(EventType.AddManager, GetManager);

        //复杂操作
        MessageServer.AddListener<GameObject,string,bool>(ReturnMessageType.GetGameObject, GetGameObjectFromPool);
        MessageServer.AddListener<GameObject, string, bool>(ReturnMessageType.GetUIObject, GetUIObjectFromPool);
    }

    public GameObject GetGameObjectFromPool(string path, bool isSetParent)
    {
        GameObject item = m_ObjectPool.GetGameObject(path, FactoryType.GameObFactory, isSetParent);
        item.SetActive(true);
        return item;
    }

    public GameObject GetUIObjectFromPool(string path, bool isSetParent)
    {
        GameObject item = m_ObjectPool.GetGameObject(path, FactoryType.UIFactory, isSetParent);
        item.SetActive(true);
        return item;
    }

    public void GetAndSetGameObject(string path, Action<GameObject> delegateItem)
    {
        GameObject item = m_ObjectPool.GetGameObject(path, FactoryType.GameObFactory, false);
        item.SetActive(true);
        if(delegateItem != null)
        {
            delegateItem(item);
        }
    }

    public void GetAndSetUIObject(string path, Action<GameObject> delegateItem)
    {
        GameObject item = m_ObjectPool.GetGameObject(path, FactoryType.UIFactory, false);
        item.SetActive(true);
        if (delegateItem != null)
        {
            delegateItem(item);
        }
    }

    public void PushObjectToPool(GameObject item)
    {
        m_ObjectPool.PushObject(item);
    }

    public void GetManager(string path)
    {
        GameRoot.Instance.AddManagerToRoot(m_ObjectPool.GetGameObject(path,FactoryType.ManagerFactory,false));
    }

    public void GetAudioClip(string path,bool isLoop)
    {
        GameRoot.Instance.PlayMusicOrBG(m_ResourceManager.GetAudioClipResource(path), isLoop);
    }

}
