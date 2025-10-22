using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[DefaultExecutionOrder(0)]
public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 3f;
    public int baseMaxHealth = 100;
    public float baseDamage = 10f;
    public float baseFireRate = 0.8f;
    public int baseProjectiles = 2;

    public KeyCode toggleKey = KeyCode.Mouse1;
    private bool aimAtCursor = false;

    public Transform projectileOrigin;
    public GameObject projectilePrefab;

    private Rigidbody2D rb;
    private float fireTimer = 0f;
    private int currentHealth;
    private float currentMaxHealth;

    private float tempFireRateMultiplier = 1f;
    private int extraProjectiles = 0;

    public int level = 1;
    public float currentXP = 0f;
    public float xpToNext = 50f;

    private HealthBar healthBar;

    private HitFlashEffect hitFlashEffect;

    void Awake()
    {
        hitFlashEffect = GetComponent<HitFlashEffect>();
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.transform.localScale = new Vector3(0, 0, 0);
        currentMaxHealth = baseMaxHealth * PersistentUpgrades.Instance.stats.maxHealthMultiplier;
        currentHealth = Mathf.CeilToInt(currentMaxHealth);
        ApplyPermanentStats();
    }

    void ApplyPermanentStats()
    {
        baseSpeed *= PersistentUpgrades.Instance.stats.speed;
        baseDamage *= PersistentUpgrades.Instance.stats.damage;
        baseFireRate /= PersistentUpgrades.Instance.stats.fireRateMultiplier;
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused) return;
        if (Input.GetKeyDown(toggleKey))
        {
            aimAtCursor = !aimAtCursor;
            Debug.Log("Modo de disparo cambiado: " + (aimAtCursor ? "Apuntado con cursor" : "Direcciones preestablecidas"));
        }
        HandleMovement();
        HandleFiring();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(h, v).normalized * baseSpeed;
    }

    void HandleFiring()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= baseFireRate * tempFireRateMultiplier)
        {
            fireTimer = 0f;
            FireProjectiles();
        }
    }

    void FireProjectiles()
    {
        int total = baseProjectiles + extraProjectiles;
        if (aimAtCursor)
        {
            
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorld - projectileOrigin.position).normalized;

            
            SpawnProjectile(direction);
        }
        else
        {
            if (total <= 2)
            {
                SpawnProjectile(Vector2.left);
                SpawnProjectile(Vector2.right);
            }
            else
            {
                float step = 360f / total;
                for (int i = 0; i < total; i++)
                {
                    float rad = step * i * Mathf.Deg2Rad;
                    Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                    SpawnProjectile(dir);
                }
            }
        }
    }

    void SpawnProjectile(Vector2 dir)
    {
        var p = Instantiate(projectilePrefab, projectileOrigin.position, Quaternion.identity);
        p.GetComponent<Projectile>().Initialize(dir, baseDamage);
    }

    public void TakeDamage(int dmg)
    {
        healthBar.transform.localScale = new Vector3(1, 1, 0);
        currentHealth -= dmg;
        hitFlashEffect.TriggerHitFlash();
        healthBar.UpdateBar(currentHealth / currentMaxHealth);
        if (currentHealth <= 0)
        {
            UIManager.Instance.OnPlayerDeath();
            gameObject.SetActive(false);
        }
    }

    public void AddXP(float xp)
    {
        currentXP += xp;
        if (currentXP >= xpToNext)
        {
            currentXP -= xpToNext;
            LevelUp();
            xpToNext *= 1.5f;
        }
        ExperienceBar.Instance.UpdateBar(currentXP / xpToNext);
    }

    void LevelUp()
    {
        level++;
        GameManager.Instance.PauseGame();
        UIManager.Instance.ShowLevelUpOptions(this);
    }


    public void ApplyTemporaryUpgrade_TurnToProjectiles(int extra) { extraProjectiles += extra; GameManager.Instance.ResumeGame(); }
    public void ApplyTemporaryUpgrade_FireRateMultiplier(float mult) { tempFireRateMultiplier *= mult; GameManager.Instance.ResumeGame(); }
    public void ApplyTemporaryUpgrade_MaxHealthIncrease(int add) { currentHealth += add; GameManager.Instance.ResumeGame(); }
    public void ApplyTemporaryUpgrade_DamageMultiplier(float mult) { baseDamage *= mult; GameManager.Instance.ResumeGame(); }
    public void ApplyTemporaryUpgrade_MoveSpeedMultiplier(float mult) { baseSpeed *= mult; GameManager.Instance.ResumeGame(); }

}
