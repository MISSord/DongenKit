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
    public Vector3 m_ShopNpc;

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
        MessageServer.AddListener(EventType.EndGame, GameOver);
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
        
        NewLevelMap();
    }

    public void NewLevelMap()
    {
        LevelNum++;
        playerCamera = Camera.main.gameObject.AddComponent<PlayerCamera>();
        playerCamera.Init();
        player.transform.position = m_MapManager.MakeNewMap();
        MessageServer.Broadcast(EventType.OpenDoor);
    }

    public override void EnableManager()
    {
        base.EnableManager();
    }

    private void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        if (playState == null)
            return;
        if (InputManager.Pause) //If player press pause button
        {
            InputManager.Pause = false; //Unpress
            isPaues = true;
            MessageServer.Broadcast(EventType.StopGame);
        }
        if (GameManager.Instance.playState.isLive)
        {
            if (InputManager.Health) //If player press helath button
            {
                InputManager.Health = false; //Unpress
                GameManager.Instance.playState.Health(); //Player health hp
            }
        }
    }

    //GameOver method
    public void GameOver()
    {
        isGameOver = true; //Game status is false, and all actions on scene is stop
    }

    public void NextLevel()
    {
        if(LevelNum == 5)
        {
            MessageServer.Broadcast(EventType.GameFinish);
        }
        else
        {
            Action loaded = () =>
                    {
                        m_MapManager.PushMapToPool();
                        NewLevelMap();
                    };
            ScenesServer.Instance.AsyncLoadScene(BaseData.FirstGameScene, loaded);
            //Load next level
        }
    }

    public void ReturnMainMenu()
    {
        GameRoot.Instance.ChangeSceneState(new MainMenuState());
        SaveServer.Save();
    }
}
