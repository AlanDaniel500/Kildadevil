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
        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
