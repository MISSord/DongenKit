using System;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ResourceManager 
{
    public string AudioloadPath = "Audio/";
    public string SpriteloadPath = "Sprites/";

    private Object[] m_Weapons2;
    private Object[] m_Weapons3;
    private Object[] m_DungeonTile;

    public void Init()
    {
        m_Weapons2 = Resources.LoadAll(SpriteloadPath + "Weapons/Weapons2");
        m_Weapons3 = Resources.LoadAll(SpriteloadPath + "Weapons/Weapons3");
        m_DungeonTile = Resources.LoadAll(SpriteloadPath + "DungeonTile");
    }


    public AudioClip GetAudioClipResource(string path)
    {
        AudioClip item = null;
        string finalPath = AudioloadPath + path;
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

    public Sprite GetSpriteResource(string path)
    {
        Sprite item = null;
        string finalPath = SpriteloadPath + path;
        if (path != null)
        {
            if (finalPath.Contains("Weapons2"))
            {
                foreach(Object m_sprite in m_Weapons2)
                {
                    if (m_sprite.name == path.Remove(0, 17))
                    {
                        item = (Sprite)m_sprite;
                        break;
                    }
                }
            }
            if (finalPath.Contains("Weapons3"))
            {
                foreach (Object m_sprite in m_Weapons3)
                {
                    if (m_sprite.name == path.Remove(0, 17))
                    {
                        item = (Sprite)m_sprite;
                        break;
                    }
                }
            }
            if (finalPath.Contains("DungeonTile"))
            {
                foreach (Object m_sprite in m_DungeonTile)
                {
                    if (m_sprite.name == path.Remove(0, 12))
                    {
                        item = (Sprite)m_sprite;
                        break;
                    }
                }
            }
        }

        if (item == null)
        {
            Debug.Log("获取" + path + "物品失败");
            Debug.Log("地址为：" + finalPath);
            return null;
        }
        return item;
    }
}

