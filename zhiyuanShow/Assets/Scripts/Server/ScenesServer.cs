using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesServer : MonoBehaviour
{
    public static ScenesServer Instance; //Singleton

    [Header("Parameters")]
    public string sceneToLoad; //Set this parameter to the name of the scene you want to go to
    private Action prgCB = null;
    public string m_currentSceneName;

    //Singleton
    public void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Instance = new ScenesServer();
        }
    }

    //Method to add a scene to the background
    public void LoadAdditiveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        m_currentSceneName = sceneName;
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
                GameRoot.Instance.ShowDownLoadUI(false);
            }
        });
    }

    private void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }
}

