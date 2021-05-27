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
                    gameRoot.AddManagerToRoot(BaseData.AudioManager);
                    gameRoot.AddManagerToRoot(BaseData.SceneManager);
                    gameRoot.AddManagerToRoot(BaseData.GameManager);
                };
        if (ScenesManager.Instance.continueGame)
        {
            gameRoot.LevelNum = 1;
            ScenesManager.Instance.AsyncLoadScene(BaseData.SecondGameScene, done);
        }
        else
        {
            ScenesManager.Instance.AsyncLoadScene(BaseData.FirstGameScene,done);
        }
        gameRoot.AddManagerToRoot(BaseData.AudioManager);
        gameRoot.AddManagerToRoot(BaseData.SceneManager);
        gameRoot.AddManagerToRoot(BaseData.GameManager);
        gameRoot.AddManagerToRoot(BaseData.UIManager);

        base.EnterScene();
    }
}
