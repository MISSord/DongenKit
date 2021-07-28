using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Coin : Item
    {
        public void OnPickedUp() //Method Pick Up item
        {
            PlayerStats.Instance.money++; //Player +1 to money
            MessageServer.Broadcast(EventType.UpdateUI); //Update UI
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            onPickedUp += OnPickedUp; //Add event to parent
            base.OnTriggerEnter2D(collision); //Parent method
        }
    }

