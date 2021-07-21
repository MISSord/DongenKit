using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 纯粹的工厂，只生产不存放
/// </summary>
public class BaseFactory : IBaseFactory
{
    protected Dictionary<string, GameObject> factoryDic = new Dictionary<string, GameObject>();

    protected string loadPath;

    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    public virtual void Init()
    {
        //AssetBundle.LoadFromFileAsync(Application.dataPath + "/AB");
    }

    public GameObject GetGameObject(string path)
    {
        GameObject go = null;
        string itemPath = loadPath + path;
        if (factoryDic.ContainsKey(itemPath))
        {
            go = factoryDic[itemPath];
        }
        else
        {
            go = Resources.Load<GameObject>(itemPath);
            factoryDic.Add(itemPath, go);
        }
        if(go == null)
        {
            Debug.Log(path + "的资源获取失败了");
            Debug.Log("获取资源失败，路径：" + itemPath);
            return null;
        }
        GameObject item = GameRoot.Instance.InstantiateGameObject(go);
        return item;
    }
}
