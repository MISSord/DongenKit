﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonKIT;

public class Gun : MonoBehaviour, IGun
{
    public float interval;
    public GameObject bulletPrefab;
    public GameObject shellPrefab;
    protected Transform muzzlePos;
    protected Transform shellPos;
    protected Vector2 mousePos;
    protected Vector2 direction;
    protected float timer;
    protected float flipY;
    protected Animator animator;
    protected PlayerCombatManager playerCombatManager;

    public virtual void Init()
    {
        playerCombatManager = PlayerCombatManager.Instance;
        animator = GetComponent<Animator>();
        muzzlePos = transform.Find("Muzzle");
        shellPos = transform.Find("BulletShell");
        flipY = transform.localScale.y;
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

    protected virtual void Fire()
    {
        animator.SetTrigger("Shoot");

        // GameObject bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = muzzlePos.position;

        float angel = Random.Range(-5f, 5f);
        bullet.GetComponent<Bullet>().SetSpeedAndDamage(Quaternion.AngleAxis(angel, Vector3.forward) * direction, 
            UnityEngine.Random.Range(BaseData.bulletMinDamage,BaseData.bulletMaxDamage));

        // Instantiate(shellPrefab, shellPos.position, shellPos.rotation);
        GameObject shell = ObjectPool.Instance.GetObject(shellPrefab);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}
