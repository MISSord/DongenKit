using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    private Button NewGameBtn;
    private Button ContinueBtn;
    private Button QuitBtn;

    public override void Init()
    {
        base.Init();
        NewGameBtn = GameObject.Find("NewGame").transform.GetComponent<Button>();
        NewGameBtn.onClick.AddListener(()=> {
            gameRoot.ChangeSceneState(new GameSceneState(gameRoot));
        });
        ContinueBtn = GameObject.Find("Continue").transform.GetComponent<Button>();
        ContinueBtn.onClick.AddListener(() => {
            GameRoot.Instance.continueGame = true;
            gameRoot.ChangeSceneState(new GameSceneState(gameRoot));
        });
        QuitBtn = GameObject.Find("Quit").transform.GetComponent<Button>();
        QuitBtn.onClick.AddListener(() => { 
            Application.Quit();
        });
    }
}
