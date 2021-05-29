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
    public PlayerCombatManager playerCombatManager;
    public PlayerController playerController;
    public PlayerCamera playerCamera;

    public UIGameManager uiManager;

    public int LevelNum = 0;

    public Transform player;

    NextLevelDoor nextLevelDoor; //Next level door object

    [Header("Settings")]
    public bool isGameOver = false;
    public bool isPaues = false;
    public bool levelComplete;
    public GameObject[] EnemyList;
    public AIStats[] EnemyStateList; 

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
        isGameOver = false;
        isPaues = false;
        playState.Init();
        playerCombatManager.Init(); 
        playerController.Init();
        base.Init();

    }

    public override void EnableManager()
    {
        InitEnemy();
        base.EnableManager();
    }

    #region 关于怪物

    public void InitEnemy()
    {
        EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        EnemyStateList = new AIStats[EnemyList.Length];
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
    }

    #endregion


    private void Start()
    {
        GameObject NextDoor = GameRoot.Instance.GetGameOb("Level/" + BaseData.NextDoorGameOb);
        NextDoor.transform.position = BaseData.NextLevelDoolPosititon;
        nextLevelDoor = NextDoor.GetComponent<NextLevelDoor>();
        InitEnemy();
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
            uiManager.ShowPauseMenu(); //Show UI pause screen
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
        //isGameOver = true; //Game status is false, and all actions on scene is stop
        playState.isLive = false;
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
        int lvlID = GameRoot.Instance.LevelNum + 1; //Level id + 1
        GameRoot.Instance.LevelNum++;
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
