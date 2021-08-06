using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : Item
{
    public void OnPickedUp() //Method Pick Up item
    {
        GameManager.Instance.playState.money++; //Player +1 to money
        MessageServer.Broadcast(EventType.UpdateUI); //Update UI
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        onPickedUp += OnPickedUp; //Add event to parent
        base.OnTriggerEnter2D(collision); //Parent method
        MessageServer.Broadcast<GameObject>(EventType.PushToPool, transform.gameObject);
    }

    private void Update()
    {
        transform.DOMove(GameManager.Instance.player.position, 3.0f);
    }
}

