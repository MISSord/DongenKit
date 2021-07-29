using System;
using UnityEngine;
using System.Collections.Generic;


public class PlayerCombatManager : MonoBehaviour
{
    public static PlayerCombatManager instance = null;

    public Transform player;

    [Header("Parameters")]
    float timeBtwShots;
    public float startTimeBtnShots;

    public List<IGun> guns = new List<IGun>();
    public List<GameObject> gunGameObjects = new List<GameObject>();
    public int gunNum;

    public void Init()
    {
        player = transform.gameObject.transform;
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            gunGameObjects.Add(transform.GetChild(1).GetChild(i).gameObject);
            gunNum = 0;
        }   
        GunInit();
    }

    private void GunInit()
    {
        for (int i = 0; i < gunGameObjects.Count ; i++)
        {
            if (gunGameObjects[i] == null)
            {
                continue;
            }
            gunGameObjects[i].transform.localPosition = BaseData.gunLocalPosition;
            gunGameObjects[i].transform.localScale = BaseData.gunScale;
            IGun item = gunGameObjects[i].GetComponent<IGun>();
            item.Init();
            guns.Add(item);
            gunGameObjects[i].gameObject.SetActive(false);
        }
        gunGameObjects[gunNum].SetActive(true);
    }

    private void Update() //Every frame
    {
        if (PlayerStats.Instance.isLive && !GameManager.Instance.isPaues) //If pause disable, and is game
        {
            SwitchGun();
            guns[gunNum].UpdateGunPosture();
            if (InputManager.Attack)
            {
                guns[gunNum].Shoot();
            }
        }
    }

    /*
    //Attack method
    void Attack()
    {
        //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

        if (InputManager.Attack)
        {
            return;
        }
        else
        {
            timeBtwShots = 0;
        }

#elif UNITY_ANDROID || UNITY_IOS //mobile

            if (VirtualJoystick.isAttack)
            {
                return;
            }
            else
            {
                timeBtwShots = 0;
            }
#endif
    }

    //Range attack method
    void RangeAttack()
    {
        GameObject rangeWeapon = Instantiate(rangeWeaponPrefab); //spawn weapon
        rangeWeapon.transform.position = transform.position; //Set weapon postion

        //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

        Vector3 mousePosition = Input.mousePosition; //Cache mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); //calculate the mouse position relative to the screen and the world

        Vector2 dir = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y); //calculate angle

        rangeWeapon.transform.up = dir; //Set angle rotation

#elif UNITY_ANDROID || UNITY_IOS //mobile

            float z = 0;

            float forX = VirtualJoystick.joystickAttackDir.x;
            float forY = VirtualJoystick.joystickAttackDir.z;

            //We perform calculations to determine the angle of flight of our weapon.
            if (forY > 0)
            {
                z = (float)(Math.Acos(forX / Math.Sqrt(Math.Pow(forX, 2) + Math.Pow(forY, 2))));
                z = z * (float)(180 / Math.PI) - 90;
            }
            else
            {
                z = (float)(Math.Acos(forX / Math.Sqrt(Math.Pow(forX, 2) + Math.Pow(forY, 2))));
                z = 360 - (z * (float)(180 / Math.PI)) - 90;
            }
            //Rotating
            rangeWeapon.transform.rotation = Quaternion.Euler(0, 0, z);
#endif
    }

    //Attack by rate method
    void AttackByRate()
    {
        if (timeBtwShots <= 0)
        {
            RangeAttack(); //Attack
            timeBtwShots = startTimeBtnShots; //set timer again
        }
        else
        {
            timeBtwShots -= Time.deltaTime; //timer - 1 sec
        }
    }
    */
    void SwitchGun()
    {
        if(gunGameObjects.Count == 1)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gunGameObjects[gunNum].SetActive(false);
            if (--gunNum < 0)
            {
                gunNum = guns.Count - 1;
            }
            gunGameObjects[gunNum].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            gunGameObjects[gunNum].SetActive(false);
            if (++gunNum > guns.Count - 1)
            {
                gunNum = 0;
            }
            gunGameObjects[gunNum].SetActive(true);
        }
    }

    public void AddWeapon(GameObject weapon)
    {
        gunGameObjects.Add(weapon);
        gunNum = gunGameObjects.Count - 1;
        GunInit();
    }

    public void ThrowWeapon()
    {

    }
}

