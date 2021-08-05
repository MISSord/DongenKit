﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIStats : MonoBehaviour
{
    SingltonMap m_singlton;
    public AIController aiController;
    AICanvas aICanvas;
    SpriteRenderer aiSprite;
    public int ID = 0;
    private bool isInit = false;
    public bool isAttackState = false;

    public delegate void DeathAction(); // AI Death Event
    public event DeathAction onDeath;

    [HideInInspector]
    public DamageEffect damageEffect; //Visual damage effect

    [Header("Settings")]
    public DoubleFloat HP; //DoubleFloat(currentHP,maxHP)

    private void Init()
    {
        aiSprite = GetComponentInChildren<SpriteRenderer>();
        aICanvas = GetComponentInChildren<AICanvas>();
        aICanvas.Init(this);
        aiController = GetComponent<AIController>();
        
        onDeath = () => {
            m_singlton = null;
            ID = 0;
            MessageServer.Broadcast<int>(EventType.EnemyDeath, ID);
            AssetServer.Instance.PushObjectToPool(this.transform.gameObject);
        };
    }

    public bool SetData(int ab, int m_ID, SingltonMap sing)
    {
        if(m_singlton != null)
        {
            return false;
        }
        m_singlton = sing;
        HP = new DoubleFloat(ab, ab);
        ID = m_ID;
        if (!isInit)
        {
            Init();
            isInit = true;
        }
        aiSprite.color = Color.white;
        aICanvas.UpdateUI();
        aiController.canMove = false;
        return true;
    }

    public void StartGame()
    {
        aiController.canMove = true;
        aiController.playerPos = GameManager.Instance.player;
    }

    public void ContinueGame()
    {
        aiController.canMove = true;
    }

    public void StopGame()
    {
        aiController.canMove = false;
    }

    //Сaused by taking damage
    public void TakingDamage(float damage)
    {
        aiController.isAttacked = true; //sends AI that he was attacked
        HP.current -= damage; //damage
        aICanvas.UpdateUI(); //Update AI ui (hp bar)
        MessageServer.Broadcast<string, bool>(EventType.PlayMusicOrBG, BaseData.EnemyDamage, false); //play damage sound
        if (HP.current <= 0) //if HP < 0 Death
        {
            Death();
            return;
        }
        StartCoroutine(damageEffect.Damage(aiSprite)); 
        
        //Start damage effect
    }

    public void Death()
    {
        if (onDeath != null)
            onDeath(); // Death event
    }

}

