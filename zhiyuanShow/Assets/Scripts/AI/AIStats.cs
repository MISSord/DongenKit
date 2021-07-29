using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIStats : MonoBehaviour
{
    SingltonMap m_singlton;
    public AIController aiController;
    AICanvas aICanvas;
    SpriteRenderer aiSprite;
    AudioSource audioSource;
    public int ID = 0;

    public bool isAttackState = false;

        //Event
    public delegate void DeathAction(); // AI Death Event
    public event DeathAction onDeath;

    [HideInInspector]
    public DamageEffect damageEffect; //Visual damage effect

    [Header("Settings")]
    public DoubleFloat HP; //DoubleFloat(currentHP,maxHP)

    public void Init(int ab, int m_ID, SingltonMap sing)
    {
        m_singlton = sing;
        aiSprite = GetComponentInChildren<SpriteRenderer>();
        aICanvas = GetComponentInChildren<AICanvas>();
        aiController = GetComponent<AIController>();
        HP = new DoubleFloat(ab,ab);
        ID = m_ID;
        onDeath = () => {
            MessageServer.Broadcast<int>(EventType.EnemyDeath, ID);
            MessageServer.Broadcast<GameObject>(EventType.PushToPool,this.transform.gameObject);
        };
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
        StartCoroutine(damageEffect.Damage(aiSprite)); 
        if (HP.current <= 0) //if HP < 0 Death
        {        
            Death();
        }
        //Start damage effect
    }

    void Death()
    {
        if (onDeath != null)
            onDeath(); // Death event
    }

}

