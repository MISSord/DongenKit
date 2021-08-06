using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIMage : AICombat
{
    [Header("Prefabs")]
    public GameObject rangeWeapon; //Prefab Throwing Weapons

    [Header("Parametrs")]
    float timeBtwShots; //time between shots
    public float startTimeBtnShots; // Start time between shots
    private bool canMove = false;


    protected override void Update()
    {
        base.Update();
        CheckAttackRadius();
    }

    void CheckAttackRadius()
    {
        if (player.transform.position == null)
        {
            return;
        }
        if (Vector2.Distance(transform.position, player.transform.position) < radiusAttack) //If a player is in radiusAttack
        {
            AttackByRate(); //Attack
        }
    }
    //Method of attack
    public override void RangeAttack(Transform target)
    {
        base.RangeAttack(target);
    }

    //AttackByRate method
    void AttackByRate()
    {
        if (timeBtwShots <= 0)
        {
            RangeAttack(player.transform); //Spawn weapon
            timeBtwShots = startTimeBtnShots;//Set time to start again
        }
        else
        {
            timeBtwShots -= Time.deltaTime;//Time minus 1 sec
        }

    }
}


