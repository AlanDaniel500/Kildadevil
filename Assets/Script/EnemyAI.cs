using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;


    [Header("References")]
    private Transform player;

    private void Start()
    {
        // Busca al jugador por su tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag 'Player' en la escena.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Calcula la dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Mueve al enemigo hacia el jugador
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // Opcional: voltear el sprite según la dirección
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}

