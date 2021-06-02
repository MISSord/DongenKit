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

    private FactoryManager factoryManager;
    private ObjectPool objectPool;
    private GameObject GamePool;
    public BaseSceneState currentSceneState;
    private GameObject loadingUI;

    private ScenesServer sceneserver;
    private AudioServer audioServer;
    private SaveServer saveServer;

    public int LevelNum = 0;
    public bool continueGame;

    private Dictionary<string, GameObject> ManagerGameObDict = new Dictionary<string, GameObject>();
    private Dictionary<string, IBaseManager> ManagerDict = new Dictionary<string, IBaseManager>();

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform);
        factoryManager = new FactoryManager();
        factoryManager.Init();

        objectPool = ObjectPool.Instance;
        objectPool.Init();
        GamePool = objectPool.pool;

        ManagerDict.Clear();

        currentSceneState = new MainMenuState(this);
        currentSceneState.EnterScene();

        loadingUI = transform.GetChild(0).gameObject;
        loadingUI.SetActive(false);

        //服务类
        sceneserver = transform.GetComponent<ScenesServer>();
        sceneserver.Init();
        audioServer = transform.GetComponent<AudioServer>();
        audioServer.Init();
        saveServer = transform.GetComponent<SaveServer>();


    }

    public void PlayMusicOrBG(string path, bool isLoop)
    {
        AudioClip item =  factoryManager.audioFactory.GetResourceFactory(path);
        if (isLoop)
        {
            audioServer.PlayBGround(item);
        }
        else
        {
            audioServer.PlayMusic(item);
        }
    }

    public GameObject InstantiateGameObject(GameObject item)
    {
        GameObject itemOne = GameObject.Instantiate(item);
        return itemOne;
    }

    public GameObject GetGameOb(string path)
    {
        GameObject item = factoryManager.factoryDict[FactoryType.GameObFactory].GetGameObject(path);
        return item;
    }

    public void PushGameObToPool(GameObject item)
    {
        objectPool.PushObject(item);
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
            objectPool.PushObject(item.Value);
        }
        ManagerGameObDict.Clear();
        ManagerDict.Clear();
    }

    public void AddManagerToRoot(string name)
    {
        GameObject item = factoryManager.factoryDict[FactoryType.ManagerFactory].GetGameObject(name);
        item.transform.SetParent(this.transform);
        ManagerGameObDict.Add(name, item);
    }

    public void ShowDownLoadUI(bool isTrue)
    {
        loadingUI.SetActive(isTrue);
    }

    public GameObject GetUI(string name)
    {
        GameObject item = factoryManager.factoryDict[FactoryType.UIFactory].GetGameObject(name);
        return item;
    }
}
