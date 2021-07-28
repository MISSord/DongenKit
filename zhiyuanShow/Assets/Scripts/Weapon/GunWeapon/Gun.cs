﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonKIT;
using System;

public class Gun : MonoBehaviour, IGun
{
    //发射间隔
    public float interval;
    protected string bulletname;
    protected string shellname;
    protected Transform muzzlePos;
    protected Transform shellPos;
    protected Vector2 mousePos;
    protected Vector2 direction;
    
    protected float timer;
    protected float flipY;
    protected Animator animator;
    protected PlayerCombatManager playCombatManager;

    public virtual void Init()
    {
        animator = GetComponent<Animator>();

        muzzlePos = transform.Find("Muzzle");
        muzzlePos.localPosition = BaseData.MuzzlePosition;

        shellPos = transform.Find("BulletShell");
        if(shellPos != null)
        {
            shellPos.localPosition = BaseData.BulletShellPosition;
        }
        
        flipY = transform.localScale.y;
        bulletname = BaseData.Bullet;
        shellname = BaseData.BulletShell;
        playCombatManager = null;//GameManager.Instance.playerCombatManager;
    }
    
    public virtual void UpdateGunPosture()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);

        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;
    }

    public virtual void Shoot()
    {
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                timer = 0;
        }

        if (timer == 0)
        {
            timer = interval;
            Fire();
        }
    }

    private Action<GameObject> FireBullet;
    private Action<GameObject> item;
    protected virtual void Fire()
    {
        animator.SetTrigger("Shoot");

        // GameObject bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
         FireBullet = (GameObject bullet) => {
            bullet.transform.position = muzzlePos.position;
            bullet.transform.rotation = Quaternion.identity;

            float angel = UnityEngine.Random.Range(-5f, 5f);
            bullet.GetComponent<Bullet>().SetSpeedAndDamage(Quaternion.AngleAxis(angel, Vector3.forward) * direction,
                UnityEngine.Random.Range(BaseData.bulletMinDamage, BaseData.bulletMaxDamage));
        };

        MessageServer.Broadcast<string, Action<GameObject>>(EventType.GetAndSetGameObject, bulletname, FireBullet);
        FireBullet.Clone();

        // Instantiate(shellPrefab, shellPos.position, shellPos.rotation);
        item = (GameObject shell) => { 
            shell.transform.position = shellPos.position;
            shell.transform.rotation = shellPos.rotation;};
        MessageServer.Broadcast<string,Action<GameObject>>(EventType.GetAndSetGameObject,shellname, item);
    }
}
