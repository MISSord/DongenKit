using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneState : BaseSceneState
{
    public GameSceneState(GameRoot Root) : base(Root)
    {
    }

    public override void EnterScene()
    {
        Action done = () =>
                {
                };
        if (GameRoot.Instance.continueGame)
        {
            gameRoot.LevelNum = 1;
            ScenesServer.Instance.AsyncLoadScene(BaseData.SecondGameScene, done);
        }
        else
        {
            ScenesServer.Instance.AsyncLoadScene(BaseData.FirstGameScene,done);
        }
        gameRoot.AddManagerToRoot(BaseData.AudioManager);
        gameRoot.AddManagerToRoot(BaseData.GameManager);
        gameRoot.AddManagerToRoot(BaseData.UIManager);
        base.EnterScene();
    }
}
