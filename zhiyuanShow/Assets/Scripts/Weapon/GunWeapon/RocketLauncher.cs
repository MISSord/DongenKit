using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Gun
{
    public int rocketNum = 3;
    public float rocketAngle = 30;

    public override void Init()
    {
        base.Init();
        bulletname = BaseData.Rocket;
        transform.localPosition = new Vector3(-0.05f, 0.01f, 0);
        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    protected override void Fire()
    {
        animator.SetTrigger("Shoot");
        StartCoroutine(DelayFire(.2f));
    }

    IEnumerator DelayFire(float delay)
    {
        yield return new WaitForSeconds(delay);

        int median = rocketNum / 2;
        for (int i = 0; i < rocketNum; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletname);
            bullet.transform.position = muzzlePos.position;
            bullet.transform.rotation = Quaternion.identity;

            if (rocketNum % 2 == 1)
            {
                bullet.transform.right = Quaternion.AngleAxis(rocketAngle * (i - median), Vector3.forward) * direction;
            }
            else
            {
                bullet.transform.right = Quaternion.AngleAxis(rocketAngle * (i - median) + rocketAngle / 2, Vector3.forward) * direction;
            }
            bullet.GetComponent<Rocket>().SetTarget(mousePos);
        }
    }
}
