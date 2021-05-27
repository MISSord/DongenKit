using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public int bulletNum = 3;
    public float bulletAngle = 15;


    protected override void Fire()
    {
        animator.SetTrigger("Shoot");

        int median = bulletNum / 2;
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletname);
            bullet.transform.position = muzzlePos.position;

            if (bulletNum % 2 == 1)
            {
                bullet.GetComponent<Bullet>().SetSpeedAndDamage(Quaternion.AngleAxis(bulletAngle * (i - median), Vector3.forward) * direction
                    , UnityEngine.Random.Range(BaseData.bulletMinDamage, BaseData.bulletMaxDamage));
            }
            else
            {
                bullet.GetComponent<Bullet>().SetSpeedAndDamage(Quaternion.AngleAxis(bulletAngle * (i - median) + bulletAngle / 2, Vector3.forward) * direction
                    , UnityEngine.Random.Range(BaseData.bulletMinDamage, BaseData.bulletMaxDamage));
            }
        }

        GameObject shell = ObjectPool.Instance.GetObject(shellname);
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;
    }
}
