using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager
{
    public static Vector2 dir { get { return Vector2.ClampMagnitude( new Vector2(Horizontal, Vertical),1f); } }
    public static Vector2 dirOne = VirtualJoystick.joystickMoveDir;

    public static bool isBlend { 
        get {
            if(dir == Vector2.zero)
            {
                return true;
            }
            return false;
        } 
    }

    //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

    public static float Vertical { get { return Input.GetAxis("Vertical"); } }  //Variable of vertical controller
    public static float Horizontal { get { return Input.GetAxis("Horizontal"); } } //Variable of Horizontal controller

#elif UNITY_ANDROID || UNITY_IOS //mobile
        public static float Vertical { get { return VirtualJoystick.joystickMoveDir.z; } }  //Variable of vertical controller
        public static float Horizontal { get { return VirtualJoystick.joystickMoveDir.x; } } //Variable of Horizontal controller
#endif

    public static Vector3 MousePosition { get { return Input.mousePosition; } } //Mouse position

    public static float MouseXPositon //Only X position of the mouse
    {
        get
        {
            float x = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
            return x;
        }
    }

    //List of Controls 
    public static bool Attack { get { return Input.GetButton(InputSettings.AttackKey); } } // Input.GetKey(InputSettings.YourKey);
    public static bool AttackDown { get { return Input.GetButtonDown(InputSettings.AttackKey); } }
    public static bool AttackUp { get { return Input.GetButtonUp(InputSettings.AttackKey); } }

    public static bool UpWeapon { get { return Input.GetKey(InputSettings.UpWeaponKey); } }
    public static bool NextWeapon { get { return Input.GetKey(InputSettings.NextWeaponKey); } }

    //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

    public static bool Interaction { get { return Input.GetKeyDown(InputSettings.InteractionKey); } set { } }

#elif UNITY_ANDROID || UNITY_IOS //mobile

        public static bool Interaction { get; set; }

#endif
    public static bool Pause { get { return Input.GetKeyDown(InputSettings.PauseKey); } set { } }

    public static bool Health { get { return Input.GetKeyDown(InputSettings.HealthKey); } set { } }

}

public class InputSettings
{
    //List of KeyCodes
    public static KeyCode InteractionKey { get { return KeyCode.E; } } //Here you can add or edit control settings
    public static string AttackKey { get { return "Attack"; } } //string for Unity Input system(Edit-Projects settings-Input) 
    public static KeyCode PauseKey { get { return KeyCode.Escape; } }
    public static KeyCode HealthKey { get { return KeyCode.Alpha1; } }
    public static KeyCode UpWeaponKey { get { return KeyCode.Q; } }
    public static KeyCode NextWeaponKey { get { return KeyCode.E; } }
}

public enum SwitchWeaqon
{
    UpWeapon,
    NextWeapon,
}
