using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float baseSpeed = 1.5f;
    public float maxHealth = 30f;
    public int contactDamage = 10;
    public int xpValue = 5;

    private float health;
    private Transform player;

    void Awake()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsPaused) return;
        if (player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.Translate(dir * baseSpeed * Time.deltaTime, Space.World);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    void Die()
    {
        PlayerController pc = player?.GetComponent<PlayerController>();
        if (pc != null) pc.AddXP(xpValue);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController pc = collision.collider.GetComponent<PlayerController>();
        if (pc != null) pc.TakeDamage(contactDamage);
    }

}
