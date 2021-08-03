using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIController : MonoBehaviour
{
    [HideInInspector] 
    public Transform playerPos; //cached player position
    Animator animator;

    [Header("Sprite AI")]
    public Transform aiSprite;

    Vector3 startPosition; //coordinate position to return when isReturnToStartPos

    [Header("Radius")]
    public float radiusAttack; //the radius at which AI begins to attack the player
    public float radiusStop; //radius at which AI stops

    [Header("Move speed")]
    public float moveSpeed; // AI move speed

    [Header("Settings")]
    public bool isReturnToStartPos; //if true, AI will return to the starting position when the player goes beyond radiusFollow
    public bool infinitiFollow; //if true, AI will follow the player endlessly.
    public bool canMove = false;
    [HideInInspector] 
    public bool isAttacked;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position; //cached start position
    }

    private void Update()
    {
        if (canMove) // check for pause and game state
        {
            Animation(); //Animation logic
            Move(); // Move logic
        }
    }
    void Animation()
    {
        animator.SetBool("Move", canMove); // Set bool in animator
    }
    void Move()
    {
        if (Vector2.Distance(transform.position, playerPos.position) > radiusAttack) //If the player has entered the pursuit radius
        {
            isAttacked = true;
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime); // Begin to follow
            Rotation();
        }
        else if(Vector2.Distance(transform.position, playerPos.position) < radiusStop)
        {
            //transform.position = Vector2.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime); // Begin to follow
            Rotation();
        }

        if (isAttacked) // If we were attacked
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, moveSpeed * Time.deltaTime);// Begin to follow
            Rotation();
        }
    }
    // Rotation(true - look at player, false loock at target)
    void Rotation()
    {
        if (playerPos.position.x - transform.position.x > 0)
        {
            aiSprite.localScale = new Vector3(1, 1, 1);
        }
        //lock left
        else if (playerPos.position.x - transform.position.x < 0)
        {
            aiSprite.localScale = new Vector3(-1, 1, 1);
        }

    }

    //Method for drawing radius
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
        Gizmos.DrawWireSphere(transform.position, radiusStop);
    }

}
