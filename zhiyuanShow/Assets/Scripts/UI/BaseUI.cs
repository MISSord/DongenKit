using System;
using System.Collections.Generic;
using UnityEngine;


public class BaseUI : MonoBehaviour, IBaseUI
{
    [HideInInspector]
    public BaseManager baseManager;

    public virtual void Init()
    {

    }

    public virtual void UpdateUI()
    {
        
    }

    public virtual void ExitUI()
    {

    }
}

public interface IBaseUI
{
    void Init();

    void UpdateUI();

    void ExitUI();
}

