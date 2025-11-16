using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 3f;
    public float damage = 10f;
    private Vector2 dir;

    public void Initialize(Vector2 direction, float dmg)
    {
        dir = direction.normalized;
        damage = dmg;
        Destroy(gameObject, lifeTime);
    }

    void Update() => transform.Translate(dir * speed * Time.deltaTime, Space.World);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BossController>() != null)
        {
            BossController boss = other.GetComponent<BossController>();
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (other.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
