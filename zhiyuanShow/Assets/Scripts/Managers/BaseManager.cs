using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseManager
{
    void Init();

    void EnableManager();

    void DisableManager();
}

public class BaseManager : MonoBehaviour, IBaseManager
{
    public bool isInit = false;
    public virtual void DisableManager()
    {
        
    }

    public virtual void EnableManager()
    {
        
    }

    public virtual void Init()
    {
        if (isInit)
        {
            EnableManager();
            return;
        }
        isInit = true;
        EnableManager();
    }
}
