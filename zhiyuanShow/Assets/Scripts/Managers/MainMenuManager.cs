using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class MainMenuManager : BaseManager
{
    [Header("Components")]
    public AnimatorManager mainMenuAnimatorManager; //Animation manager

    bool isAnyKeyDown; //splash screen status

    public GameObject loadGameBtn;
    private MainMenuUI mainMenuUI;

    public override void Init()
    {
        mainMenuAnimatorManager = GameObject.Find(BaseData.AnimatorManager).GetComponent<AnimatorManager>();

        mainMenuUI = transform.GetComponent<MainMenuUI>();
        mainMenuUI.Init();
        mainMenuUI.baseManager = this;

        loadGameBtn = GameObject.Find("Continue");
        loadGameBtn.SetActive(false);

        if (PlayerPrefs.GetString(BaseData.GameLevel) != "")
            loadGameBtn.SetActive(true);

        MessageServer.Broadcast<string,bool>(EventType.PlayMusicOrBG, BaseData.MainMenuBG,true);
        base.Init();
    }

    private void Update()
    {
        if (!isAnyKeyDown && Input.anyKeyDown) //if any key down
        {
            SplashScreenClose(); //Splash screen disable
        }
    }

    //Splash screen disable method
    void SplashScreenClose()
    {
        isAnyKeyDown = true;
        mainMenuAnimatorManager.PlayPlayableDirector(mainMenuAnimatorManager.timelineAssets[1], DirectorWrapMode.None); //Play main menu animation
    }
    

}
