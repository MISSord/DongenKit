using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonKIT;

/// <summary>
/// 整个游戏最高层的管理器
/// </summary>
public class GameRoot : MonoBehaviour
{
    [HideInInspector]


    public void Awake()
    {
        DontDestroyOnLoad(transform);
    }
}
