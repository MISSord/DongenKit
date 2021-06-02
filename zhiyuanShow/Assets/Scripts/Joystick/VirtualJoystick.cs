﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public enum JoystickType { Move, Attack } //Enum for check type

    [Header("Parameters")]
    public JoystickType joystickType;

    Image bgImg; //Border of UI Joystick
    Image joyImg; //Joystic

    public static Vector3 joystickMoveDir;

    public static bool isAttack;

    private void Start()
    {
        bgImg = GetComponent<Image>();
        joyImg = transform.GetChild(0).GetComponent<Image>(); //Get first child Image to chache joystick
    }

    private void Update()
    {
        if (GameManager.Instance.isUsekeyboard)
        {
            joystickMoveDir = InputManager.dir;
            joyImg.rectTransform.anchoredPosition = new Vector3(InputManager.dir.x * (bgImg.rectTransform.sizeDelta.x / 3), InputManager.dir.y * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }

    //Method triggers when finger presses screen
    public void OnPointerDown(PointerEventData eventData)
    {
        if (joystickType == JoystickType.Attack) //if atack joystick
            isAttack = true; //attacking

        OnDrag(eventData);
    }

    //The method works when the finger moves
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (GameManager.Instance.isUsekeyboard)
        {
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            //We carry out calculations depending on the size of the joystick
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            Vector3 dir = new Vector3(x, y, 0);
            dir = (dir.magnitude > 1) ? dir.normalized : dir;

            //Check joystick
            switch (joystickType)
            {
                case JoystickType.Move:
                    joystickMoveDir = dir;
                    break;
            }

            //Move child image
            joyImg.rectTransform.anchoredPosition = new Vector3(dir.x * (bgImg.rectTransform.sizeDelta.x / 3), dir.y * (bgImg.rectTransform.sizeDelta.y / 3));

        }
        joystickMoveDir = pos;
    }

    //The method works when the finger up
    public void OnPointerUp(PointerEventData eventData)
    {
        if (joystickType == JoystickType.Attack)
            isAttack = false; //Stop attack

        //reset child image position
        joystickMoveDir = Vector3.zero;
        joyImg.rectTransform.anchoredPosition = Vector3.zero;
    }

}
