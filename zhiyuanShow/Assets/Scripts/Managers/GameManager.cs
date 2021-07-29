using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
    public PlayerController playerController;
    public PlayerCamera playerCamera;

    public MapManager m_MapManager;
    public PlayerCamera m_playercamera;

    public bool isUsekeyboard = true;

    public int LevelNum = 0;

    public Transform player;

    [Header("Settings")]
    public bool isGameOver = false;
    public bool isPaues = false;
    public bool levelComplete = false;

    [Header("Enemy")]
    public Vector3 playerBorn;

    public override void Init()
    {
        instance = this;

        m_MapManager = transform.GetComponent<MapManager>();
        m_MapManager.Init();

        isGameOver = false;
        isPaues = false;

        MessageServer.AddListener(EventType.FinishSceneLoad, StartGame);
        MessageServer.AddListener(EventType.NextLevel, NextLevel);
        //Mes
        base.Init();
    }

    public void StartGame()
    {
        if(player != null)
        {
            return;
        }

        player = MessageServer.Broadcast<GameObject,string,bool>(ReturnMessageType.GetGameObject, BaseData.Player, true).transform;
        playState = player.transform.GetComponent<PlayerStats>();
        playerController = player.transform.GetComponent<PlayerController>();
        playState.Init();
        playerController.Init();


        playerCamera = Camera.main.gameObject.AddComponent<PlayerCamera>();
        playerCamera.Init();
        player.transform.position = m_MapManager.MakeNewMap();

        MessageServer.Broadcast(EventType.OpenDoor);
    }

    public override void EnableManager()
    {
        base.EnableManager();
    }

    private void Start()
    {
        //GameObject NextDoor = MessageServer.Broadcast<GameObject>(ReturnMessageType.GetGameObject, BaseData.NextDoorGameOb);
        //NextDoor.transform.position = BaseData.NextLevelDoolPosititon;
        //nextLevelDoor = NextDoor.GetComponent<NextLevelDoor>();
    }

    private void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        if (InputManager.Pause) //If player press pause button
        {
            InputManager.Pause = false; //Unpress
            isPaues = true;
            MessageServer.Broadcast(EventType.StopGame);
        }
        if (PlayerStats.Instance.isLive)
        {
            if (InputManager.Health) //If player press helath button
            {
                InputManager.Health = false; //Unpress
                PlayerStats.Instance.Health(); //Player health hp
            }
        }
    }

    //GameOver method
    public void GameOver()
    {
        isGameOver = true; //Game status is false, and all actions on scene is stop
        playState.isLive = false;
        
    }

    //Complete level method
    public void LevelComplete()
    {
        MessageServer.Broadcast(EventType.NextLevel);
        levelComplete = true; //Set bool for Check door status
        //nextLevelDoor.lockedDoor = false; //Unlock door
        //nextLevelDoor.CheckLockStatus(); //Check door status
    }

    public void NextLevel()
    {
        Action loaded = () =>
        {
            StartGame();
        };
        if(ScenesServer.Instance.m_currentSceneName == BaseData.FirstGameScene)
        {
            ScenesServer.Instance.AsyncLoadScene(BaseData.SecondGameScene, loaded);
        }
        else
        {
            ScenesServer.Instance.AsyncLoadScene(BaseData.FirstGameScene, loaded);
        }
        m_MapManager.PushMapToPool();
         //Load next level
    }

    public void PlayisDead()
    {
        playState.isLive = false;
    }

    public void ReturnMainMenu()
    {
        //GameRoot.Instance.currentSceneState = new MainMenuState(GameRoot.Instance);
        //SaveManager.Save();
        //GameRoot.Instance.currentSceneState.EnterScene();
    }
}
