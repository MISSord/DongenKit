using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIGameManager : BaseManager
{
    public PlayerStats playerStats;
    DialogManager dialogManager;

    [Header("Components")]
    List<GameObject> HPUIObjects = new List<GameObject>(); //UI HP list
    public Transform hpParent; //HP parent, position for spawn
    public GameObject hpIconPrefab; //HP prefab for spawn
    public Sprite hpActiveSprite, hpDisableSprite; //Sprites for HP( 1 hpActive - you have 1 hp, 1 hpDisabled - you have taken damage  )
    private InteractionCanvas m_interactionCanvas;//显示伤害和提示的Canvas

    public Text moneyText, bottleText, keyText; //UI text

    [Header("Screens GameObjects")]
    public GameObject dialogGO, shopGO;
    public GameObject pauseGo;
    public GameObject gameoverGO;
    public GameObject mobileUIGO;
    public Transform canvasTransf;

    public bool isPause;

    public event EventHandler dialogClosed; //Close dialog event

    //Singleton method

    public override void Init()
    {
        
        //Check active platform
#if UNITY_ANDROID || UNITY_IOS //mobile 

            mobileUIGO.SetActive(true); //Enable mobile UI
#endif
        dialogManager = GetComponent<DialogManager>();
        MessageServer.AddListener<DialogConfig>(EventType.ShowDialog, ShowDialogMenu);

        m_interactionCanvas = transform.Find("Canvas").GetComponent<InteractionCanvas>();
        m_interactionCanvas.Init();
        dialogClosed += CloseShopMenu; //Add event

        MessageServer.AddListener(EventType.FinishSceneLoad, StartGame);
        base.Init();
    }

    public void StartGame()
    {
        playerStats = PlayerStats.Instance; //Set playerstats in static object of PlayerStats
        UpdateUI(); //UpdateUI
    }

    //Update ui method
    public void UpdateUI()
    {
        if (playerStats == null)
        {
            playerStats = GameManager.Instance.playState;
        }
        UpdateHP(); //Update HP 
        moneyText.text = playerStats.money.ToString(); //Update ui money text
        bottleText.text = playerStats.bottles.ToString(); //Update ui bottle text
        keyText.text = playerStats.doorKeys.Count.ToString();
    }

    //Update hp method
    public void UpdateHP()
    {
        //Loop for clear old hp
        for (int i = 0; i < HPUIObjects.Count; i++)
        {
            MessageServer.Broadcast<GameObject>(EventType.PushToPool, HPUIObjects[i]);
        }
        HPUIObjects.Clear(); //Clear list

        //Loop for spawn new 
        for (int i = 0; i < playerStats.HP.max; i++)
        {

            GameObject item = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetUIObject, BaseData.HealthPoint, false);
            item.transform.SetParent(hpParent);
            item.transform.localScale = Vector3.one;

            Image hpIcon = item.GetComponent<Image>(); //Spawn prefab 
            if (playerStats.HP.current > i) //check player hp
            {
                hpIcon.sprite = hpActiveSprite; //Set Active hp
            }
            else
            {
                hpIcon.sprite = hpDisableSprite; //Set disable hp
            }
            HPUIObjects.Add(hpIcon.gameObject); //Add object to list 
        }
    }

    //Show dialog menu method
    public void ShowDialogMenu(DialogConfig dialogConfig)
    {
        isPause = true; //set pause
        dialogGO.SetActive(true); //Show dialog screen gameobject
        dialogManager.SetDialogConfig(dialogConfig); //set config to dialog
    }

    //Show shop menu method
    public void ShowShopMenu()
    {
        shopGO.SetActive(true); //Show shop screen
    }

    //Close dialog method
    public void CloseDialogMenu()
    {
        isPause = false; //disable pause

        dialogClosed(this, new EventArgs()); //Activate event
        dialogGO.SetActive(false); //Disable dialog screen
    }

    //Close shop menu method
    public void CloseShopMenu(object sender, EventArgs e)
    {
        shopGO.SetActive(false); //Disable shop screen
    }

    //Pause method
    public void ShowPauseMenu()
    {
        pauseGo.SetActive(!pauseGo.activeSelf); //Reverse pause screen active status 
    }

    //UI GameOver method
    public void GameOver()
    {
        gameoverGO.SetActive(true); //gameover screen enable
    }

    public void ExitGame()
    {
        //GameManager.Instance.ReturnMainMenu();
        
    }

    //Check active platform
#if UNITY_ANDROID || UNITY_IOS //mobile 
        public void HealthBtn()
        {
            playerStats.Health(); //Player health hp
        }
        public void InteractiveBtn()
        {
            InputManager.Interaction = true;
        }
#endif
}

