using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonKIT;

/// <summary>
/// 整个游戏最高层的管理器
/// </summary>
public class GameRoot : MonoBehaviour
{
    [HideInInspector]
    private static GameRoot instance = null;
    public static GameRoot Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameRoot();
            }
            return instance;
        }
    }

    private GameObject GamePool;
    public BaseSceneState currentSceneState;
    private GameObject loadingUI;

    private ScenesServer m_sceneserver;
    private AudioServer m_audioServer;
    private SaveServer m_saveServer;
    private AssetServer m_AssetServer;

    public bool continueGame;

    private Dictionary<string, GameObject> ManagerGameObDict = new Dictionary<string, GameObject>();
    private Dictionary<string, IBaseManager> ManagerDict = new Dictionary<string, IBaseManager>();

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform);


        //服务类
        m_sceneserver = transform.GetComponent<ScenesServer>();
        m_sceneserver.Init();
        m_audioServer = transform.GetComponent<AudioServer>();
        m_audioServer.Init(m_audioServer);
        m_saveServer = transform.GetComponent<SaveServer>();
        m_saveServer.Init();
        m_AssetServer = transform.GetComponent<AssetServer>();
        m_AssetServer.Init();


        MessageServer.AddListener<BaseSceneState, bool>(EventType.InitGame, NewOrLoadGame);
        MessageServer.AddListener(EventType.InitManagerDic, InitManagerDict);
        MessageServer.AddListener(EventType.ClearDic, ClearDict);

        ManagerDict.Clear(); 

        loadingUI = transform.GetChild(0).gameObject;
        loadingUI.SetActive(false);

        currentSceneState = new MainMenuState();
        currentSceneState.EnterScene();

    }

    public void PlayMusicOrBG(AudioClip item,bool isLoop)
    {
        if (isLoop)
        {
            m_audioServer.PlayBGround(item);
        }
        else
        {
            m_audioServer.PlayMusic(item);
        }
    }

    public void InitManagerDict()
    {
        foreach(var item in ManagerGameObDict)
        {
            IBaseManager itemManager = item.Value.transform.GetComponent<IBaseManager>();
            if (itemManager == null)
            {
                Debug.Log("获取面板上的IBaseManager脚本失败");
                continue;
            }
            ManagerDict.Add(item.Key, itemManager);
            itemManager.Init();
        }
    }

    public void NewOrLoadGame(BaseSceneState item, bool isLoad)
    {
        if (isLoad)
        {
            continueGame = true;
        }
        ChangeSceneState(item);
    }

    public void ChangeSceneState(BaseSceneState item)
    {
        currentSceneState.ExitScene();
        currentSceneState = item;
        currentSceneState.EnterScene();
    }

    public void ClearDict()
    {
        foreach (var item in ManagerGameObDict)
        {
            IBaseManager itemManager = ManagerDict[item.Key];
            if (itemManager == null)
            {
                Debug.Log("获取面板上的IBaseManager脚本失败");
            }
            itemManager.DisableManager();
            m_AssetServer.PushObjectToPool(item.Value);
        }
        ManagerGameObDict.Clear();
        ManagerDict.Clear();
    }

    public void AddManagerToRoot(GameObject item)
    {
        item.transform.SetParent(this.transform.GetChild(1).transform);
        ManagerGameObDict.Add(item.name, item);
    }

    public void ShowDownLoadUI(bool isTrue)
    {
        loadingUI.SetActive(isTrue);
    }
}
