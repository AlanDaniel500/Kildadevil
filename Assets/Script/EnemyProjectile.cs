using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 4f;
    public float lifeTime = 3f;
    public int damage = 10;
    private Vector2 dir;

    public void Initialize(Vector2 direction, int dmg)
    {
        dir = direction.normalized;
        damage = dmg;
        Destroy(gameObject, lifeTime);
    }

    void Update() => transform.Translate(dir * speed * Time.deltaTime, Space.World);

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController p = other.GetComponent<PlayerController>();
        if (p != null)
        {
            p.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
