using System;
using UnityEngine;
using System.IO;


public class AudioFactory : IBaseResourceFactory<AudioClip>
{
    public string loadPath = "Audio/";

    public void Init()
    {

    }


    public AudioClip GetResourceFactory(string path)
    {
        AudioClip item = null;
        string finalPath = loadPath + path;
        if (path != null)
        {
            item = Resources.Load<AudioClip>(finalPath);
        }

        if(item == null)
        {
            Debug.Log("获取" + path + "物品失败");
            Debug.Log("地址为：" + finalPath);
            return null;
        }
        return item;
    }
}

