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

    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public DamageEffect damageEffect; //Damage effect

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

    public bool canMove = true;

    public void Init()
    {
        audioSource = GameManager.Instance.player.GetComponent<AudioSource>();
        playerSprite = GameManager.Instance.player.GetChild(0).GetComponent<SpriteRenderer>();
        timeToDamage = BaseData.DamageTime;
        damageEffect = new DamageEffect();
        if (instance == null)
        {
            instance = this;
        }

        //if (ScenesManager.Instance.continueGame)
        //    SaveManager.Load();
    }

    //Taking damage method
    public void TakingDamage()
    {
        if (!isDamaged) // if player is't damaged
        {
            isDamaged = true; //block damage
            StartCoroutine(timeDamage()); //set timer to next damage

            HP.current -= 1; //HP - 1

            GameManager.Instance.uiManager.UpdateUI(); //Update UI

            StartCoroutine(damageEffect.Damage(playerSprite)); //Damage effect

            AudioManager.Instance.Play(audioSource, AudioManager.Instance.playerDamage, false); //play damage sound

            if (HP.current <= 0) //If hp < 0
            {
                Death(); //Lose 
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

            AudioManager.Instance.Play(audioSource, AudioManager.Instance.drinkBottle, false); //play drink sound

            GameManager.Instance.uiManager.UpdateUI(); //Update UI
        }
    }
    //Death method
    void Death()
    {
        GameManager.Instance.GameOver(); //Game over in gamemanager
        Destroy(gameObject); //Destroy this GameObject
    }
    IEnumerator timeDamage()
    {
        yield return new WaitForSeconds(timeToDamage); //Wait timeToDamage
        isDamaged = false; //can damage again
    }
}



