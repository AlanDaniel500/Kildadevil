using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [Header("Estadísticas básicas")]
    public float baseSpeed = 1.5f;
    public float maxHealth = 30f;
    public int contactDamage = 10;
    public int xpValue = 5;

    [Header("Knockback al recibir daño")]
    public float knockbackForce = 3f;      
    public float knockbackDuration = 0.15f;

    [Header("Knockback al jugador")]
    public float playerKnockbackForce = 4f;

    private float health;
    private Transform player;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;

    private bool canDamage = true;
    public float damageCooldown = 0.5f;

    void Awake()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsPaused) return;
        if (player == null || isKnockedBack) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * baseSpeed;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
            return;
        }

        
        if (player != null)
        {
            Vector2 knockDir = (transform.position - player.position).normalized;
            StartCoroutine(ApplyKnockback(knockDir));
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockedBack = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
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
        if (pc != null)
        {
            TryDamagePlayer(pc);


            Rigidbody2D playerRb = pc.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockDir = (playerRb.position - rb.position).normalized;
                playerRb.AddForce(knockDir * playerKnockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController pc = collision.collider.GetComponent<PlayerController>();
        if (pc != null)
        {
            TryDamagePlayer(pc);
        }
    }

    private void TryDamagePlayer(PlayerController pc)
    {
        if (canDamage)
        {
            pc.TakeDamage(contactDamage);

            canDamage = false;
            StartCoroutine(DamageCooldownRoutine());
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
