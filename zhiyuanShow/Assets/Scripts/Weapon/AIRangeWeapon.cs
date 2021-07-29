using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIRangeWeapon : RangeWeapon
{

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        if (collider.gameObject.tag == "Player") //if contact with player
        {
            Damage(); //Player damaged
        }
    }

    //Damage method
    void Damage()
    {
        MessageServer.Broadcast(EventType.PlayDamge);
        Destroying(); //Destroyng gameobject
    }

}
