using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneState : IBaseSceneState
{
    protected GameRoot gameRoot;

    public BaseSceneState(GameRoot Root)
    {
        gameRoot = Root;
    }

    public virtual void EnterScene()
    {
        gameRoot.InitManagerDict();
    }

    public virtual void ExitScene()
    {
        gameRoot.ClearDict();
    }
}

public interface IBaseSceneState
{
    void EnterScene();

    void ExitScene();
}
