using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家数据
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public PlayerCombatManager m_combatManager;

    //[HideInInspector] 
    public DamageEffect damageEffect; //Damage effect

    [Header("Variables")]
    public DoubleInt HP = null;
    public int money = 0;
    public int bottles = 0;
    public Dictionary<int, bool> doorKeys = new Dictionary<int, bool>();

    [Header("Parameters")]
    public float timeToDamage; //Time for pause between AI damage
    bool isDamaged;

    [Header("Graphics")]
    public SpriteRenderer playerSprite; //Player sprite

    public bool isLive = true;

    public void Init(int m_currentHp,int m_Hp, int m_money, int m_bottles, List<string> m_gun)
    {
        HP = new DoubleInt(m_currentHp, m_Hp);
        money = m_money;
        bottles = m_bottles;
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        timeToDamage = BaseData.DamageTime;
        m_combatManager = transform.GetComponent<PlayerCombatManager>();
        m_combatManager.Init(m_gun);
        MessageServer.AddListener(EventType.PlayDamge, TakingDamage);
    }

    public void DestroySelf()
    {
        m_combatManager.DestroySelf();
        m_combatManager = null;
    }

    //Taking damage method
    public void TakingDamage()
    {
        if (!isDamaged) 
        {
            isDamaged = true;
            StartCoroutine(timeDamage());

            HP.current -= 1; 

            MessageServer.Broadcast(EventType.UpdateUI); //Update UI

            StartCoroutine(damageEffect.Damage(playerSprite)); //Damage effect

            MessageServer.Broadcast<string, bool>(EventType.PlayMusicOrBG,BaseData.PlayerDamage, false); //play damage sound

            if (HP.current <= 0) //If hp < 0
            {
                Death(); //Lose 
            }
        }
    }
    //Health method
    public void Health()
    {
        if (HP.current == HP.max)
            return;
        if (bottles > 0)
        {
            bottles--; //Bottles - 1
            HP.current++; //HP + 1
            MessageServer.Broadcast<string, bool>(EventType.PlayMusicOrBG, BaseData.UseItem, false); //play drink sound
            MessageServer.Broadcast(EventType.UpdateUI);
        }
    }

    //Death method
    void Death()
    {
        isLive = false;
        MessageServer.Broadcast(EventType.EndGame);
    }

    public PlayerInfor SaveInfor()
    {
        PlayerInfor infor = new PlayerInfor()
        {
            currentHp = HP.current,
            Hp = HP.max,
            bottle = bottles,
            coin = money,
            GuiName = m_combatManager.GetGunName(),
        };
        return infor;
    }

    IEnumerator timeDamage()
    {
        yield return new WaitForSeconds(timeToDamage); //Wait timeToDamage
        isDamaged = false; //can damage again
    }

}



