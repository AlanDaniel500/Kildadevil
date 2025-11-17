using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("References")]
    public Transform player;                    // Assign in inspector or find by tag
    public GameObject aoeWarningPrefab;         // Red semi-transparent box (SpriteRenderer)
    public GameObject projectilePrefab;         // Your bullet prefab
    public Transform firePoint;                 // Where projectiles spawn

    [Header("Attack Settings")]
    public float attackCooldown = 3f;           // Time between attacks
    public float aoeWarningTime = 1f;           // How long warning shows before damage
    public float aoeFadeOutTime = 0.5f;         // Fade out after damage
    public int aoeDamage = 30;
    public Vector2 aoeSize = new Vector2(20f, 2.5f);
    public float projectileSpeed = 8f;
    public int projectileDamage = 30;

    private float nextAttackTime = 0f;
    private int currentAttackIndex = 0;         // 0 = AOE, 1 = Projectile


    // Las 3 posiciones Y fijas (puedes ajustar estos valores según tu juego)
    float[] yPositions;
    // Lista con los índices de las posiciones (0, 1, 2)
    List<int> indices = new List<int> { 0, 1, 2 };

    public float maxHealth = 4500f;
    private float health;
    public int contactDamage = 10;
    private bool canContactDamage = true;
    public float contactDamageCooldown = 0.5f;
    private HitFlashEffect hitFlashEffect;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
            Debug.LogError("Boss can't find Player! Tag your player as 'Player'");

        // Las 3 posiciones Y fijas (puedes ajustar estos valores según tu juego)
        yPositions = new float[]
        {
            transform.position.y + 0.5f,
            transform.position.y + 3.5f,  // Ejemplo: segunda altura
            transform.position.y - 2.5f   // Ejemplo: tercera altura
        };

        health = maxHealth;
        hitFlashEffect = GetComponent<HitFlashEffect>();
        BossHealthBar.Instance.UpdateBar(health / maxHealth);
        ExperienceBar.Instance.GetComponent<CanvasGroup>().alpha = 0f;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsPaused) return;
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            // Cycle between attacks
            if (currentAttackIndex == 0)
                StartCoroutine(AOEAttack());
            else
                ProjectileAttack();

            currentAttackIndex = 1 - currentAttackIndex; // Toggle 0 - 1
        }
    }

    IEnumerator AOEAttack()
    {
        // Barajar y tomar los primeros 2 índices
        for (int i = 0; i < indices.Count; i++)
        {
            int temp = indices[i];
            int randomIndex = Random.Range(i, indices.Count);
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // Crear LISTA de AOEs al mismo tiempo
        List<GameObject> warnings = new List<GameObject>();

        // Instanciar los 2 AOEs INMEDIATAMENTE (sin esperar nada)
        for (int i = 0; i < 2; i++)
        {
            int idx = indices[i];
            Vector3 position = new Vector3(transform.position.x - 14f, yPositions[idx], transform.position.z);
            GameObject warning = Instantiate(aoeWarningPrefab, position, Quaternion.identity);

            // Setup básico
            SpriteRenderer sr = warning.GetComponent<SpriteRenderer>();
            BoxCollider2D col = warning.GetComponent<BoxCollider2D>();
            sr.color = new Color(1f, 0f, 0f, 0.1f); // Alpha inicial bajo
            warning.transform.localScale = new Vector3(aoeSize.x, aoeSize.y, 1f);
            if (col != null) col.isTrigger = true;

            warnings.Add(warning);
        }

        // Iniciar Coroutine independiente para CADA AOE
        foreach (GameObject warning in warnings)
        {
            StartCoroutine(HandleSingleAOE(warning));
        }

        yield return null; // AOEAttack termina inmediatamente
    }

    IEnumerator HandleSingleAOE(GameObject warning)
    {
        SpriteRenderer sr = warning.GetComponent<SpriteRenderer>();

        // Fade in over 0.3s
        float fadeInTime = 0.3f;
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0.1f, 0.5f, t / fadeInTime);
            sr.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        // Wait for warning duration
        yield return new WaitForSeconds(aoeWarningTime - fadeInTime);

        // FLASH + DEAL DAMAGE
        sr.color = new Color(1f, 0.3f, 0.3f, 0.8f);

        // Damage player if inside
        BoxCollider2D col = warning.GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(warning.transform.position, aoeSize, 0f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerController p = hit.GetComponent<PlayerController>();
                    if (p != null) p.TakeDamage(aoeDamage);
                }
            }
        }

        // Fade out
        for (float t = 0; t < aoeFadeOutTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0.8f, 0f, t / aoeFadeOutTime);
            sr.color = new Color(1f, 0.3f, 0.3f, alpha);
            yield return null;
        }

        Destroy(warning);
    }

    void ProjectileAttack()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        var p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        p.GetComponent<EnemyProjectile>().Initialize(dir, projectileDamage);
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController pc = collision.collider.GetComponent<PlayerController>();
        if (pc != null)
        {
            TryDamagePlayer(pc);
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
        if (canContactDamage)
        {
            pc.TakeDamage(contactDamage);

            canContactDamage = false;
            StartCoroutine(DamageCooldownRoutine());
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        yield return new WaitForSeconds(contactDamageCooldown);
        canContactDamage = true;
    }

    public void TakeDamage(float amount)
    {
        if (amount > 0)
        {
            health -= amount;
            hitFlashEffect.TriggerHitFlash();
            BossHealthBar.Instance.UpdateBar(health / maxHealth);
            if (health <= 0)
            {
                Die();
                GameManager.Instance.EndRun((int) GameManager.Instance.matchDuration);
                return;
            }
        }
    }

    void Die()
    {
        PlayerController pc = player?.GetComponent<PlayerController>();
        Destroy(gameObject);
    }
}
