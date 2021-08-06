using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasergun : Gun
{
    private GameObject effect;
    private LineRenderer laser;
    private bool isShooting;
    private bool isInit = false;

    public override void Init()
    {
        base.Init();
        laser = muzzlePos.GetComponent<LineRenderer>();
        effect = transform.Find("Effect").gameObject;
        isInit = true;
        muzzlePos.gameObject.SetActive(true);
        laser.enabled = false;
    }

    public override void  Update()
    {
        base.Update();
        if (isInit)
        {
            Shoot();
        }
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

        laser.SetPosition(0, muzzlePos.position);
        laser.SetPosition(1, hit2D.point);

        effect.transform.position = hit2D.point;
        effect.transform.forward = direction;
        RaycastHit[] m_hit;
        m_hit = Physics.RaycastAll(muzzlePos.position, direction, 100f);
        for (int i = 0; i < m_hit.Length; i++)
        {
            RaycastHit hit = m_hit[i];
            if(m_hit[i].collider.tag == "Enemy")
            {
                m_hit[i].collider.gameObject.transform.GetComponent<AIStats>().TakingDamage(20);
            }
        }
        
    }
}
