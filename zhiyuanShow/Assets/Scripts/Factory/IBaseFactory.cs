using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 获取GameObject工厂
/// </summary>
public interface IBaseFactory 
{
    GameObject GetGameObject(string path);
}


/// <summary>
/// 获取多种资源的工厂
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseResourceFactory<T>
{
    T GetResourceFactory(string path);
}
