using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Door : MonoBehaviour
{
    SpriteRenderer[] spriteRenderer;
    Collider2D[] doorColliders;

    public void Init()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        doorColliders = GetComponents<Collider2D>();

        MessageServer.AddListener(EventType.OpenDoor,OpenDoor);
        MessageServer.AddListener(EventType.CloseDoor, CloseDoor);
    }

    void OpenDoor()
    {
        for(int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].DOFade(0f, 1.0f).OnComplete(() =>
            {
                doorColliders[0].enabled = false;
            });
        }
        doorColliders[1].enabled = true;
    }

    void CloseDoor()
    {
        doorColliders[0].enabled = true;
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].DOFade(1f, 1.0f);
        }
    }

}
