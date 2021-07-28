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

    NextLevelDoor nextLevelDoor; //Next level door object

    [Header("Settings")]
    public bool isGameOver = false;
    public bool isPaues = false;
    public bool levelComplete;

    [Header("Enemy")]
    public GameObject[] EnemyList;
    public AIStats[] EnemyStateList;
    public int EnemyCount = 0;

    public override void Init()
    {
        instance = this;

        m_MapManager = transform.GetComponent<MapManager>();
        m_MapManager.Init();

        isGameOver = false;
        isPaues = false;

        MessageServer.AddListener(EventType.FinishSceneLoad, StartGame); 
        
        base.Init();
    }

    public void StartGame()
    {
        player = MessageServer.Broadcast<GameObject,string,bool>(ReturnMessageType.GetGameObject, BaseData.Player, true).transform;
        playState = new PlayerStats();
        playerController = player.transform.GetComponent<PlayerController>();
        playState.Init();
        playerController.Init();

        switch (LevelNum)
        {
            case 0:
                player.position = BaseData.Level0Player;
                break;
            case 1:
                player.position = BaseData.Level1Player;
                break;
        }

        playerCamera = Camera.main.gameObject.AddComponent<PlayerCamera>();
        playerCamera.Init();
        m_MapManager.MakeNewMap();
        //InitEnemy();

        MessageServer.Broadcast(EventType.OpenDoor);
    }

    


    public override void EnableManager()
    {
        base.EnableManager();
    }

    #region 关于怪物
    public void InitEnemy()
    {
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyStateList = new AIStats[EnemyList.Length];
        EnemyCount = EnemyList.Length;
        for(int i = 0; i < EnemyList.Length; i++)
        {
            EnemyStateList[i] = EnemyList[i].GetComponent<AIStats>();
            EnemyStateList[i].ID = i;
        }
        if(EnemyList.Length > 0)
        {
            Debug.Log("怪物初始化成功");
        }
        else
        {
            Debug.Log("怪物初始化失败");
        }
    }

    public void TakeDamageToEnemy(int i, float damage)
    {
        if(EnemyStateList[i] != null)
        {
            lock(EnemyStateList[i]){
                EnemyStateList[i].TakingDamage(damage);
            }
        }
    }

    public void EnemyDeath(int i)
    {
        EnemyStateList[i].DestroySelf();
        EnemyStateList[i] = null;
        EnemyList[i] = null;

        if (CheckIsAllDead())
        {
            MessageServer.Broadcast(EventType.LevelComplete);
        }
    }

    public bool CheckIsAllDead()
    {
        for(int i = 0; i < EnemyCount; i++)
        {
            if(EnemyStateList[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    #endregion


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
            player.transform.position = BaseData.Level1Player;
            InitEnemy();
        };
        ScenesServer.Instance.AsyncLoadScene(BaseData.SecondGameScene, loaded); //Load next level
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
