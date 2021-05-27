using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class DoorKey : Item
    {
        [Header("Settings")]
        public int keyID;

        public void OnPickedUp()
        {
            PlayerStats.Instance.doorKeys.Add(keyID, true);

            GameManager.Instance.uiManager.UpdateUI(); //Update UI
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            onPickedUp += OnPickedUp;
            base.OnTriggerEnter2D(collision);
        }
    }
