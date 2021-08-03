using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Door : MonoBehaviour
{
    SpriteRenderer[] spriteRenderer;
    Collider2D doorColliders;

    public void Init()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        doorColliders = GetComponent<Collider2D>();

        MessageServer.AddListener(EventType.OpenDoor,OpenDoor);
        MessageServer.AddListener(EventType.CloseDoor, CloseDoor);
    }

    void OpenDoor()
    {
        for(int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].DOFade(0f, 1.0f).OnComplete(() =>
            {
                doorColliders.enabled = false;
            });
        }
        
    }

    void CloseDoor()
    {

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].DOFade(1f, 1.0f).OnComplete(() =>
            {
                doorColliders.enabled = true;
            });
        }
    }

}
