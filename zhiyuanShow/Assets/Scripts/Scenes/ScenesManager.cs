using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesManager : BaseManager
{
    public static ScenesManager Instance; //Singleton

    [Header("Parameters")]
    public string sceneToLoad; //Set this parameter to the name of the scene you want to go to
    public int levelID; //Current level

    public bool continueGame;
    private Action prgCB = null;

    //Singleton
    public override void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        base.Init();
    }

    //Method to add a scene to the background
    public void LoadAdditiveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        prgCB = (() =>
        {
            float val = sceneAsync.progress;
            GameRoot.Instance.ShowDownLoadUI(true);
            if (val == 1)
            {
                if (loaded != null)
                {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.ShowDownLoadUI(true);
            }
        });
    }
}

