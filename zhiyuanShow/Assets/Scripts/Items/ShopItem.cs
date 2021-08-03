using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ShopItemInfor m_infor;

    private Button m_BuyButton;

    // Start is called before the first frame update
    public void Init(ShopItemInfor infor)
    {
        m_infor = infor;
        transform.GetChild(0).GetComponent<Image>().sprite =
                MessageServer.Broadcast<Sprite, string>(ReturnMessageType.GetSprite, m_infor.spriteId);
        transform.GetChild(1).GetComponent<Text>().text = m_infor.itemname;
        transform.GetChild(2).GetComponent<Text>().text = m_infor.price.ToString();
        m_BuyButton = transform.GetChild(4).GetComponent<Button>();
        m_BuyButton.onClick.AddListener(() => {
            MessageServer.Broadcast<int>(EventType.Buy, m_infor.ID);
        });
    }
}
