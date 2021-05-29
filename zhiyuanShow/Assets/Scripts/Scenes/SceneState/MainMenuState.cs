using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : BaseSceneState
{
    public MainMenuState(GameRoot root) : base(root)
    {

    }

    public override void EnterScene()
    {
        gameRoot.AddManagerToRoot(BaseData.MainMenuManager);
        gameRoot.AddManagerToRoot(BaseData.AudioManager);
        base.EnterScene();
    }
}
