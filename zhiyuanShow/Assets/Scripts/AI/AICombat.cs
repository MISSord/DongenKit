using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AICombat : MonoBehaviour
{
    [HideInInspector]
    public int radiusAttack;
    protected GameObject player;
    public bool canMove = true;
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        if (!canMove)
            return;
    }

    //Virtual method for Range Attack, reconfigured in child classes
    public virtual void RangeAttack(Transform target)
    {
        GameObject rangeShot = AssetServer.Instance.GetGameObjectFromPool(BaseData.FireBall, false); //Creates a weapon
        rangeShot.transform.position = transform.position; //Moves a weapon to its position
        Vector2 dir = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        rangeShot.transform.up = dir;
    }

    //Virtual method for Melee Attack, reconfigured in child classes
    public virtual void MeleeAttack(GameObject target)
    {
        PlayerStats playerStats = GameManager.Instance.playState;
        playerStats.TakingDamage();
    }

    public virtual void Death()
    {
        StopAllCoroutines();
    }
}
