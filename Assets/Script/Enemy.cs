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

    private HealthBar healthBar;

    private HitFlashEffect hitFlashEffect;

    private int type;

    private SpriteRenderer spriteRenderer;
    private Color spriteColor;

    public Transform projectileOrigin;
    public GameObject projectilePrefab;
    private float fireTimer = 0;

    void Awake()
    {
        health = maxHealth;
        hitFlashEffect = GetComponent<HitFlashEffect>();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.transform.localScale = new Vector3(0, 0, 0);
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsPaused) return;
        if (player == null || isKnockedBack) return;

        Movement();
    }

    public void Initialize(int type)
    {
        this.type = type;
        switch (this.type)
        {
            case 1:
                spriteColor = Color.red;
                break;
            case 2:
                spriteColor = Color.purple;
                break;
            case 3:
                spriteColor = Color.blue;
                rb.bodyType = RigidbodyType2D.Kinematic;
                int enemyLayer = LayerMask.NameToLayer("EnemyToEnemy");
                gameObject.layer = enemyLayer;
                break;
        }
        spriteRenderer.color = spriteColor;
    }

    void Movement()
    {
        switch (type)
        {
            case 1:
                Vector2 dir = (player.position - transform.position).normalized;
                rb.velocity = dir * baseSpeed;
                break;
            case 2:
                Vector2 dir2 = (player.position - transform.position).normalized;
                Vector2 perpendicular = new Vector2(-dir2.y, dir2.x);
                float wave = Mathf.Sin(Time.time * 5f + (2f * Mathf.PI)) * 5f;
                Vector2 finalVelocity = (dir2 * baseSpeed) + (perpendicular * wave);
                rb.velocity = finalVelocity;
                break;
            case 3:
                HandleFiring();
                break;
        }
    }

    void HandleFiring()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= 3f)
        {
            fireTimer = 0f;
            SpawnProjectile();
        }
    }

    void SpawnProjectile()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        var p = Instantiate(projectilePrefab, projectileOrigin.position, Quaternion.identity);
        p.GetComponent<EnemyProjectile>().Initialize(dir, 10);
    }

    public void TakeDamage(float amount)
    {
        if (amount > 0)
        {
            healthBar.transform.localScale = new Vector3(1, 1, 0);
            health -= amount;
            hitFlashEffect.TriggerHitFlash();
            healthBar.UpdateBar(health / maxHealth);
            if (health <= 0)
            {
                Die();
                return;
            }
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
