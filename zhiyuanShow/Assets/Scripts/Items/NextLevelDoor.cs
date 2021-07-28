using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class NextLevelDoor : MonoBehaviour
{
    CircleCollider2D collider2D;

    [Header("Components")]
    SpriteRenderer m_OpenspriteRenderer;
    Transform m_ClosespriteRenderer;
    public Sprite lockedSprite, openedSprite;
    [HideInInspector]
    public bool lockedDoor = true; //Door status

    bool inTrigger;
    InteractionTrigger interactionTrigger;

    private void Start()
    {
        interactionTrigger = GetComponent<InteractionTrigger>();
        interactionTrigger.Init();
        m_OpenspriteRenderer = GetComponent<SpriteRenderer>();
        m_ClosespriteRenderer = transform.GetChild(0);
        collider2D = GetComponent<CircleCollider2D>();

        MessageServer.AddListener(EventType.LevelComplete, OpenDoor);
        CheckLockStatus(); //Check door status
    }

    private void Update()
    {
        //if player in trigger
        if (interactionTrigger.inTrigger)
        {
            //if player press Interaction button
            if (InputManager.Interaction)
            {
                InputManager.Interaction = false; //unpress button
                if (!lockedDoor) //if door unlocked
                {
                    MessageServer.Broadcast(EventType.NextLevel);
                }
            }
        }
    }

    public void OpenDoor()
    {
        m_ClosespriteRenderer.DOLocalMoveX(1, 1.0f);
        lockedDoor = false;
        CheckLockStatus();
    }

    //检查楼梯的开关状态
    public void CheckLockStatus()
    {
        if (lockedDoor) //if door locked
        {
            collider2D.enabled = false; //trigger disabled
        }
        else
        {
            collider2D.enabled = true; //trigger enabled
        }
    }
}

