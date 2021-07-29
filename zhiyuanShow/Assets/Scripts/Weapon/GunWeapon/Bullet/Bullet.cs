using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    public float speed;
    public string explosionname;
    new private Rigidbody2D rigidbody;
    private float damageRange = 0;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        explosionname = BaseData.Explosion;
        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        StartCoroutine(DestorySelf());
    }

    IEnumerator DestorySelf()
    {
        yield return new WaitForSeconds(10f);
        ObjectManager.Instance.PushObject(gameObject);
    }

    public void SetSpeedAndDamage(Vector2 direction, float damage)
    {
        rigidbody.velocity = direction * speed;
        damageRange = damage;
    }

    Action<GameObject> exp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Obstacle")
        {
            return;
        }
        if (other.gameObject.tag == "Enemy")
        {
            AIStats enemy = other.gameObject.GetComponent<AIStats>();
            enemy.TakingDamage(damageRange);
        }
        exp = (GameObject expPrefabs) =>
        {
            expPrefabs.transform.position = transform.position;
        };
        MessageServer.Broadcast<string, Action<GameObject>>(EventType.GetAndSetGameObject, explosionname ,exp);

        ObjectManager.Instance.PushObject(gameObject);
    }
}
