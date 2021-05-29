using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonKIT;

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
    }

    public void SetSpeedAndDamage(Vector2 direction, float damage)
    {
        rigidbody.velocity = direction * speed;
        damageRange = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Obstacle")
        {
            return;
        }
        if (other.gameObject.tag == "Enemy")
        {
            AIStats enemy = other.gameObject.GetComponent<AIStats>();
            GameManager.Instance.TakeDamageToEnemy(enemy.ID, damageRange);
        }
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameObject exp = ObjectPool.Instance.GetObject(explosionname);
        exp.transform.position = transform.position;

        // Destroy(gameObject);
        ObjectPool.Instance.PushObject(gameObject);
    }
}
