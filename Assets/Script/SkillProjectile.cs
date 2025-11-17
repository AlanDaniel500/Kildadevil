using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 3f;
    public float damage = 10f;
    private Vector2 dir;

    public void Initialize(Vector2 direction, float dmg)
    {
        gameObject.transform.transform.localScale *= 1.8f;
        dir = direction.normalized;
        damage = dmg * 1.4f;
        Destroy(gameObject, lifeTime);
    }

    void Update() => transform.Translate(dir * speed * Time.deltaTime, Space.World);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BossController>() != null)
        {
            BossController boss = other.GetComponent<BossController>();
            boss.TakeDamage(damage);
            return;
        }

        if (other.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
