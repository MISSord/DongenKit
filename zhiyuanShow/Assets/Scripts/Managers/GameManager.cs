using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : BaseManager
{
    private static GameManager instance; //Singleton

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    public PlayerStats playState;
    public PlayerCombatManager playerCombatManager;
    public PlayerController playerController;
    public PlayerCamera playerCamera;

    public UIGameManager uiManager;

    public int LevelNum = 0;

    public Transform player;

    NextLevelDoor nextLevelDoor; //Next level door object

    [Header("Settings")]
    public bool isGame = true;
    public bool levelComplete;

    public override void Init()
    {
        instance = this;
        LevelNum = GameRoot.Instance.LevelNum;
        player = GameRoot.Instance.GetGameOb(BaseData.Player).transform;

        uiManager = GameObject.Find("UIManager(Clone)").transform.GetComponent<UIGameManager>();
        
        switch (LevelNum)
        {
            case 0:
                player.position = BaseData.Level0Player;
                break;
            case 1:
                player.position = BaseData.Level1Player;
                break;
        }

        playState = player.GetComponent<PlayerStats>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerController = player.transform.GetComponent<PlayerController>();
        playState.Init();   
        playerCombatManager.Init();   
        playerController.Init();
        base.Init();
    }

    private void Start()
    {
        GameObject NextDoor = GameRoot.Instance.GetGameOb("Level/" + BaseData.NextDoorGameOb);
        NextDoor.transform.position = BaseData.NextLevelDoolPosititon;
        nextLevelDoor = NextDoor.GetComponent<NextLevelDoor>();
    }

    //GameOver method
    public void GameOver()
    {
        isGame = false; //Game status is false, and all actions on scene is stop
        uiManager.GameOver(); //Show GameOver screen
    }

    //Complete level method
    public void LevelComplete()
    {
        levelComplete = true; //Set bool for Check door status
        nextLevelDoor.lockedDoor = false; //Unlock door
        nextLevelDoor.CheckLockStatus(); //Check door status
    }

    public void NextLevel()
    {
        int lvlID = ScenesManager.Instance.levelID + 1; //Level id + 1
        ScenesManager.Instance.levelID++;
        //ScenesManager.Instance.LoadLoadingScene("Lvl_" + lvlID); //Load next level

    }
}
