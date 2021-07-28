using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { Item, Coin }

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    public ItemType type; //Item type

    public delegate void PickedUpAction(); //Delegate for Pick up item
    public event PickedUpAction onPickedUp; //Pick up event

    public virtual void OnTriggerEnter2D(Collider2D collision) //If player entered in trigger
    {
        if (collision.gameObject.tag == "Player") //if its player
        {
            onPickedUp(); //Event

            switch (type)
            {
                case ItemType.Item:
                    MessageServer.Broadcast<string,bool>(EventType.PlayMusicOrBG, BaseData.ItemUp, false); //play pickup item sound
                    break;
                case ItemType.Coin:
                    MessageServer.Broadcast<string, bool>(EventType.PlayMusicOrBG, BaseData.CoinUp, false); //play pickup coin sound
                    break;
            }

            //Destroy(gameObject); // Destroy this GameObject
        }
    }

}
