using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIStats : MonoBehaviour
{
    SingltonMap m_singlton;
    private AIController aiController;
    private AICombat aiCombat;

    AICanvas aICanvas;
    SpriteRenderer aiSprite;
    public int ID = 0;
    private bool isInit = false;
    public bool isAttackState = false;

    public delegate void DeathAction(); // AI Death Event
    public event DeathAction onDeath;
    public DamageEffect damageEffect; //Visual damage effect

    [Header("Settings")]
    public DoubleFloat HP; //DoubleFloat(currentHP,maxHP)
    private int coin = 0;

    private void Init()
    {
        aiSprite = GetComponentInChildren<SpriteRenderer>();
        aICanvas = GetComponentInChildren<AICanvas>();
        aICanvas.Init(this);
        aiController = GetComponent<AIController>();
        aiCombat = GetComponent<AICombat>();

        onDeath = () => {
            m_singlton = null;
            ID = 0;
            MessageServer.Broadcast<int>(EventType.EnemyDeath, ID);
            AssetServer.Instance.PushObjectToPool(this.transform.gameObject);
        };
    }

    public bool SetData(int ab, int m_ID, SingltonMap sing, int m_coin)
    {
        if(m_singlton != null)
        {
            return false;
        }
        m_singlton = sing;
        HP = new DoubleFloat(ab, ab);
        ID = m_ID;
        coin = m_coin;
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
        aiCombat.canMove = true;
        aiController.playerPos = GameManager.Instance.player;
    }

    public void ContinueGame()
    {
        aiController.canMove = true;
        aiCombat.canMove = true;
    }

    public void StopGame()
    {
        aiController.canMove = false;
        aiCombat.canMove = false;
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
            for (int i = 0; i < coin; i++)
            {
                GameObject coin = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, BaseData.Coin, false);
                coin.transform.position = transform.position;
            }
            return;
        }
        StartCoroutine(damageEffect.Damage(aiSprite)); 
        
        //Start damage effect
    }

    public void Death()
    {
        aiCombat.Death();
        if (onDeath != null)
            onDeath(); // Death event
    }

}

