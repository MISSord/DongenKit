using System;
using System.Collections.Generic;
using UnityEngine;


public class BaseUI : MonoBehaviour, IBaseUI
{
    [HideInInspector]
    public GameRoot gameRoot;
    public virtual void Init()
    {
        gameRoot = GameRoot.Instance;
    }

    public virtual void UpdateUI()
    {
        
    }
}

public interface IBaseUI
{
    void Init();

    void UpdateUI();

    
}

