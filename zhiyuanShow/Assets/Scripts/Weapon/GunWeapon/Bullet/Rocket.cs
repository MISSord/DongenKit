using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float lerp;
    public float speed = 15;
    public string explosionname;
    new private Rigidbody2D rigidbody;
    private Vector3 targetPos;
    private Vector3 direction;
    private bool arrived;
    private float damageRange = 100;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        explosionname = BaseData.RocketExplosion;
        transform.localScale = BaseData.normalScale;
    }

    public void SetTarget(Vector2 _target)
    {
        arrived = false;
        targetPos = _target;
    }

    private void FixedUpdate()
    {
        direction = (targetPos - transform.position).normalized;

        if (!arrived)
        {
            transform.right = Vector3.Slerp(transform.right, direction, lerp / Vector2.Distance(transform.position, targetPos));
            rigidbody.velocity = transform.right * speed;
        }
        if (Vector2.Distance(transform.position, targetPos) < 1f && !arrived)
        {
            arrived = true;
        }
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
        GameObject exp = ObjectPool.Instance.GetObject(explosionname);
        exp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        exp.transform.position = transform.position;

        rigidbody.velocity = Vector2.zero;
        StartCoroutine(Push(gameObject, 0.3f));
    }

    IEnumerator Push(GameObject _object, float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.Instance.PushObject(_object);
    }
}
