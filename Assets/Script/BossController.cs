using System.Collections;
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
    public Vector2 aoeSize = new Vector2(5f, 5f);
    public float projectileSpeed = 8f;
    public int projectileDamage = 30;

    private float nextAttackTime = 0f;
    private int currentAttackIndex = 0;         // 0 = AOE, 1 = Projectile

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
            Debug.LogError("Boss can't find Player! Tag your player as 'Player'");
    }

    void Update()
    {
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
        // Spawn warning zone at boss position (or offset if you want)
        GameObject warning = Instantiate(aoeWarningPrefab, transform.position, Quaternion.identity);
        SpriteRenderer sr = warning.GetComponent<SpriteRenderer>();
        BoxCollider2D col = warning.GetComponent<BoxCollider2D>();

        // Setup
        sr.color = new Color(1f, 0f, 0f, 0.3f); // Semi-transparent red
        warning.transform.localScale = new Vector3(aoeSize.x, aoeSize.y, 1f);
        if (col != null) col.isTrigger = true;

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
}
