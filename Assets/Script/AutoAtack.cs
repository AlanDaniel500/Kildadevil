using UnityEngine;

public class AutoAtack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 3f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackInterval = 1f;

    [Header("Layer Settings")]
    [SerializeField] private LayerMask enemyLayer;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackInterval)
        {
            Attack();
            timer = 0f;
        }
    }

    private void Attack()
    {
        // Buscar enemigos dentro del rango
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                // Calcula dirección del impacto (para retroceso)
                Vector2 hitDir = (enemy.transform.position - transform.position).normalized;

                health.TakeDamage(attackDamage, hitDir);
            }
        }
    }

    // Opcional: dibujar el rango en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

