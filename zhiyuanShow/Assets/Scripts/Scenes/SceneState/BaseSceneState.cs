using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneState : IBaseSceneState
{
    public BaseSceneState(){}

    public virtual void EnterScene()
    {
        MessageServer.Broadcast(EventType.InitManagerDic);
    }

    public virtual void ExitScene()
    {
        MessageServer.Broadcast(EventType.ClearDic);
    }
}

public interface IBaseSceneState
{
    void EnterScene();

    void ExitScene();
}
