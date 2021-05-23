using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonKIT;

public class Lasergun : Gun
{
    private GameObject effect;
    private LineRenderer laser;
    private bool isShooting;

    public override void Init()
    {
        base.Init();
        laser = muzzlePos.GetComponent<LineRenderer>();
        effect = transform.Find("Effect").gameObject;
    }

    public override void Shoot()
    {

        if (InputManager.AttackDown)
        {
            isShooting = true;
            laser.enabled = true;
            effect.SetActive(true);
        }
        if (InputManager.AttackUp)
        {
            isShooting = false;
            laser.enabled = false;
            effect.SetActive(false);
        }
        animator.SetBool("Shoot", isShooting);

        if (isShooting)
        {
            Fire();
        }
    }

    protected override void Fire()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(muzzlePos.position, direction, 30);

        // Debug.DrawLine(muzzlePos.position, hit2D.point);
        laser.SetPosition(0, muzzlePos.position);
        laser.SetPosition(1, hit2D.point);

        effect.transform.position = hit2D.point;
        effect.transform.forward = -direction;
    }
}
