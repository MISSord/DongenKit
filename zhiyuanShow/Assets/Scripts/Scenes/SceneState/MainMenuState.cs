using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : BaseSceneState
{
    public MainMenuState()
    {

    }

    public override void EnterScene()
    {
        MessageServer.Broadcast<string>(EventType.AddManager, BaseData.MainMenuManager);
        base.EnterScene();
    }
}
