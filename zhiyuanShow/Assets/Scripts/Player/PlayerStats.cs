using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家数据
/// </summary>
public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance; //Singleton

    public static PlayerStats Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new PlayerStats();
            }
            return instance;
        }
    }
    public PlayerCombatManager m_combatManager;

    //[HideInInspector] 
    public DamageEffect damageEffect; //Damage effect

    [Header("Variables")]
    public DoubleInt HP = new DoubleInt(3, 3);
    public int money = 0;
    public int bottles = 0;
    public Dictionary<int, bool> doorKeys = new Dictionary<int, bool>();

    [Header("Parameters")]
    public float timeToDamage; //Time for pause between AI damage
    bool isDamaged;

    [Header("Graphics")]
    private SpriteRenderer playerSprite; //Player sprite

    public bool isLive = true;

    public void Init()
    {
        playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        timeToDamage = BaseData.DamageTime;
        damageEffect = new DamageEffect();
        m_combatManager = transform.GetComponent<PlayerCombatManager>();
        m_combatManager.Init();
    }

    //Taking damage method
    public void TakingDamage()
    {
        if (!isDamaged) 
        {

            isDamaged = true;
            StartCoroutine(timeDamage());

            HP.current -= 1; 

            MessageServer.Broadcast<PlayerStats>(EventType.UpdateUI, this); //Update UI

            StartCoroutine(damageEffect.Damage(playerSprite)); //Damage effect

            MessageServer.Broadcast<string, bool>(EventType.PlayMusicOrBG,BaseData.PlayerDamage, false); //play damage sound

            if (HP.current <= 0) //If hp < 0
            {
                Death(); //Lose 
                isLive = false;
                MessageServer.Broadcast(EventType.EndGame);
            }
        }
    }
    //Health method
    public void Health()
    {
        if (bottles > 0)
        {
            bottles--; //Bottles - 1
            HP.current++; //HP + 1
            MessageServer.Broadcast<string, bool>(EventType.PlayMusicOrBG, BaseData.UseItem, false); //play drink sound
            MessageServer.Broadcast<PlayerStats>(EventType.UpdateUI, this);
        }
    }

    //Death method
    void Death()
    {
        GameManager.Instance.GameOver(); //Game over in gamemanager
        //Destroy(gameObject); //Destroy this GameObject
    }

    IEnumerator timeDamage()
    {
        yield return new WaitForSeconds(timeToDamage); //Wait timeToDamage
        isDamaged = false; //can damage again
    }

}



