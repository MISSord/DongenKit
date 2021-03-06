using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorKey : Item
{
    [Header("Settings")]
    public int keyID;

    public void OnPickedUp()
    {
        GameManager.Instance.playState.doorKeys.Add(keyID, true);

        //GameManager.Instance.uiGameManager.UpdateUI(); //Update UI
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        onPickedUp += OnPickedUp;
        base.OnTriggerEnter2D(collision);
    }
}
