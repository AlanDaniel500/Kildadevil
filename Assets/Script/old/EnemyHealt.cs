using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float knockbackForce = 5f;

    private float currentHealth;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Recibir da�o
    public void TakeDamage(float damage, Vector2 hitDirection)
    {
        currentHealth -= damage;

        // Retroceso
        rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        // Aqu� luego puedes agregar efectos, animaciones o part�culas
    }
}
