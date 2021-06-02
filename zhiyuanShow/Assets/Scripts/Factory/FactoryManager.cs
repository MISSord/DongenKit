using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager 
{
    public GameRoot gameRoot;
    public Dictionary<FactoryType, BaseFactory> factoryDict = new Dictionary<FactoryType, BaseFactory>();
    public AudioFactory audioFactory;

    public void Init()
    {
        gameRoot = GameRoot.Instance;
        factoryDict.Add(FactoryType.GameObFactory, new GameObFactory());
        factoryDict.Add(FactoryType.ManagerFactory, new ManagerFactory());
        factoryDict.Add(FactoryType.UIFactory, new UIFactory());
        audioFactory = new AudioFactory();
    }
}

public enum FactoryType
{
    GameObFactory,
    ManagerFactory,
    UIFactory,
}
