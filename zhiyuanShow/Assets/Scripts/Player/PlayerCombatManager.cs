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

    public void Init(List<string> gun)
    {
        if(gun != null)
        {
            AddWeapons(gun);
        }
        player = transform.gameObject.transform;
        GunInit();
        MessageServer.AddListener<GameObject>(EventType.AddGun, AddWeapon);
    }

    public void AddWeapons(List<string> gun)
    {
        for(int i = 0; i < gun.Count; i++)
        {
            GameObject item = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, gun[i], true);
            AddWeapon(item);
        }
    }

    private void GunInit()
    {
        guns.Clear();
        if(gunGameObjects.Count == 0)
        {
            return;
        }
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
        if (guns.Count == 0)
            return;
        if (GameManager.Instance.playState.isLive && !GameManager.Instance.isPaues) //If pause disable, and is game
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
    }

    public void AddWeapon(GameObject weapon)
    {
        gunGameObjects.Add(weapon);
        gunNum = gunGameObjects.Count - 1;
        weapon.transform.SetParent(player.GetChild(1));
        GunInit();
    }

    public void ThrowWeapon()
    {

    }

    public void DestroySelf()
    {
        for(int i = 0; i < gunGameObjects.Count; i++)
        {
            MessageServer.Broadcast<GameObject>(EventType.PushToPool,gunGameObjects[i]);
        }
        gunGameObjects.Clear();
        guns.Clear();
    }

    public List<string> GetGunName()
    {
        List<string> item = new List<string>();
        for(int i = 0; i < gunGameObjects.Count; i++)
        {
            item.Add(gunGameObjects[i].name);
        }
        return item;
    }
}

