using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class ShopManager : BaseManager
{
    [Header("Items")]
    public List<ShopItemInfor> m_shopItemInfors;

    private Transform m_ShopFieldContent;

    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameManager.Instance.playState;
        m_ShopFieldContent = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform;
        MessageServer.AddListener<int>(EventType.Buy, Buy);
        UpdateItemInfor();
    }

    public void UpdateItemInfor()
    {
        m_shopItemInfors = MessageServer.Broadcast<List<ShopItemInfor>>(ReturnMessageType.GetShopInfor);
        for (int i = 0; i < m_shopItemInfors.Count; i++)
        {
            GameObject item = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetUIObject, BaseData.Shopitem, true);
            item.transform.parent = m_ShopFieldContent;
            item.transform.localScale = Vector3.one;
            ShopItem m_shopitem =  item.AddComponent<ShopItem>();
            m_shopitem.Init(m_shopItemInfors[i]);
        }
    }

    //Buy method
    private void Buy(int itemID)
    {
        if(playerStats.money >= m_shopItemInfors[itemID].price)
        {
            playerStats.money -= m_shopItemInfors[itemID].price; //Player money - price
            switch (m_shopItemInfors[itemID].ID) //Check item type
            {
                case 0: //Bottles type 
                    playerStats.bottles++; //Bottles + 1
                    break;
                default:
                    GameObject item = MessageServer.Broadcast<GameObject,string,bool>
                        (ReturnMessageType.GetGameObject, m_shopItemInfors[itemID].prefabName,true);
                    item.transform.position = GameManager.Instance.player.transform.position;
                    item.transform.DOMove(item.transform.position + new Vector3(UnityEngine.Random.Range(2, 5),UnityEngine.Random.Range(2, 5), 0), 1.0f).SetEase(Ease.InOutQuart);
                    break;
            }
        }

        MessageServer.Broadcast(EventType.UpdateUI); //Update UI
    }

}

[Serializable]
public class ShopItemInfor
{
    public string itemname;
    public int price;
    public string spriteId;
    public string prefabName;
    public int ID;
}




