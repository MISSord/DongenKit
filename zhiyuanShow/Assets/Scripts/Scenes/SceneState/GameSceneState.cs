using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneState : BaseSceneState
{
    public GameSceneState()
    {

    }

    public override void EnterScene()
    {
        Action done = () =>
                {
                    MessageServer.Broadcast<string>(EventType.AddManager, BaseData.GameManager);
                    MessageServer.Broadcast<string>(EventType.AddManager, BaseData.UIGameManager);
                    base.EnterScene();
                    MessageServer.Broadcast(EventType.FinishSceneLoad);
                };
     
         ScenesServer.Instance.AsyncLoadScene(BaseData.FirstGameScene, done);

    }
}
