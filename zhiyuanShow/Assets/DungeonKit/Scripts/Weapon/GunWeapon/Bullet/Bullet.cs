using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonKIT;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject explosionPrefab;
    new private Rigidbody2D rigidbody;
    private float damageRange = 0;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetSpeedAndDamage(Vector2 direction, float damage)
    {
        rigidbody.velocity = direction * speed;
        damageRange = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            AIStats enemy = other.gameObject.GetComponent<AIStats>();
            enemy.TakingDamage(damageRange);
        }
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
        exp.transform.position = transform.position;

        // Destroy(gameObject);
        ObjectPool.Instance.PushObject(gameObject);
    }
}
