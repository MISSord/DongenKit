using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工厂基类
/// </summary>
public class BaseFactory : IBaseFactory
{
    //游戏资源
    protected Dictionary<string, GameObject> factoryDic = new Dictionary<string, GameObject>();

    protected string loadPath;

    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    /// <summary>
    /// 获取未实例化的游戏物体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public virtual GameObject GetGameObject(string path)
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
        return go;
    }
}
