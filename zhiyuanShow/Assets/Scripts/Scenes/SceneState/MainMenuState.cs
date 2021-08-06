using System;
using UnityEngine;

public class MainMenuState : BaseSceneState
{
    public MainMenuState()
    {

    }

    public override void EnterScene()
    {
        ScenesServer.Instance.AsyncLoadScene(BaseData.MainMenuScene,
            () =>{
                MessageServer.Broadcast<string>(EventType.AddManager, BaseData.MainMenuManager);
                base.EnterScene();
            });
    }
}
