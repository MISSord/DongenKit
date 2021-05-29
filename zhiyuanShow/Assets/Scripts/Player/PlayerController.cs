using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制人物的移动
/// </summary>
public class PlayerController : MonoBehaviour
{
    //Components
    public Animator playerAnimator;
    Rigidbody2D rigidbody2d;

    [Header("Components")]
    public GameObject playerSprite;
    public SpriteRenderer playerSpriteRenderer;

    [Header("Parameters")]
    public float moveSpeed;
    public void Init()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerAnimator = transform.GetChild(0).GetComponentInChildren<Animator>();
        SaveManager.Save(); //Save level state
    }

    void FixedUpdate()
    {
        if (PlayerStats.Instance.isLive && !GameManager.Instance.isPaues) //If pause disable, and is game
        {
            CheckActions();
        }
    }
    //Actions method
    void CheckActions()
    {
        Rotation();  //Rotation of player 旋转人物
        Move(); //Player move 移动
        Animation(); //Player animation 播放动画
    }

    //Move method
    void Move()
    {
        //Movement of the character depending on the values InputManager.Horizontal, InputManager.Vertical
        transform.Translate(new Vector3(InputManager.Horizontal, InputManager.Vertical, 0) * moveSpeed * Time.deltaTime);
    }

    //Rotation method
    void Rotation()
    {
        //Check active platform
#if UNITY_STANDALONE // PC,WIN,MAC

        //If mouse in right side of screen
        if (InputManager.MouseXPositon > 0.5f)
        {
            playerSprite.transform.localScale = new Vector3(1, 1, 1); // character look right
        }
        //If mouse in left side of screen
        else if (InputManager.MouseXPositon < 0.5f)
        {
            playerSprite.transform.localScale = new Vector3(-1, 1, 1); // character look left
        }

#elif UNITY_ANDROID || UNITY_IOS //mobile
            //If stick in right side
            if (InputManager.Horizontal > 0)
            {
                playerSprite.transform.localScale = new Vector3(1, 1, 1); // character look right
            }
            //If stick in left side
            else if (InputManager.Horizontal < 0)
            {
                playerSprite.transform.localScale = new Vector3(-1, 1, 1); // character look left
            }
#endif
    }

    //Animation method
    void Animation()
    {
        if (InputManager.Horizontal != 0 || InputManager.Vertical != 0) //if character is move
        {
            playerAnimator.SetBool("Move", true); //Animator set move 
        }
        else
        {
            playerAnimator.SetBool("Move", false); //Animator reset move
        }
    }

    //Inputs method
    
}

