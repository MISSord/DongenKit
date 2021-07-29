using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody2D))]
public class RangeWeapon : MonoBehaviour
{
    [Header("Parameters")]
    public float moveSpeed = 1;
    private bool isCanMove = true;
    public DoubleFloat damageRange = new DoubleFloat(10, 20);

    private void Start()
    {
        StartCoroutine(DestroyByTime()); //Timer to destroy
        MessageServer.AddListener(EventType.StopGame, CantMove);
        MessageServer.AddListener(EventType.ContinueGame, CanMove);
    }

    public void CanMove()
    {
        isCanMove = true;
    }

    public void CantMove()
    {
        isCanMove = false;
    }

    private void FixedUpdate()
    {
        if (isCanMove)
        {
            Move();
        }
    }

    //Move method
    void Move()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime); //Move
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) //If in contact with an obstacle
    {
        if (collider.gameObject.tag == "Obstacle") //if obstacle
        {
            Destroying();
        }
    }
    //Destroy method
    public void Destroying()
    {
        AssetServer.Instance.PushObjectToPool(this.gameObject);
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(5); //Destroy gameobject after 5 sec
        Destroy(gameObject);
    }
}
