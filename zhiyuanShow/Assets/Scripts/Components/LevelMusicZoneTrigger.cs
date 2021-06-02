using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelMusicZoneTrigger : MonoBehaviour
{
    public AudioClip music;

    public void OnTriggerEnter2D(Collider2D collision) //If player entered in trigger
    {
        if (collision.gameObject.tag == "Player") //if its player
        {
            GameRoot.Instance.PlayMusicOrBG(BaseData.OpenDoor,false);
        }
    }

    public void OnTriggerExit2D(Collider2D collision) //If player entered in trigger
    {
        if (collision.gameObject.tag == "Player") //if its player
        {
            //GameRoot.Instance.PlayMusicOrBG(BaseData.OpenDoor,false);
        }
    }
}
