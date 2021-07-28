using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class HelmetOfStrength_Item : Item
    {

        public void OnPickedUp()
        {
            PlayerStats playerStats = PlayerStats.Instance;

            playerStats.HP = new DoubleInt(5,5);
            //GameManager.Instance.uiGameManager.UpdateUI();
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            onPickedUp += OnPickedUp;
            base.OnTriggerEnter2D(collision);
        }

    }

