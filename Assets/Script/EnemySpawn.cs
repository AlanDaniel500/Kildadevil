using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float horizontalDistance = 10f;
    [SerializeField] private float verticalVariation = 1.5f;
    [SerializeField] private int maxEnemies = 10;

    [Header("References")]
    [SerializeField] private Transform player;

    private float timer = 0f;
    private int currentEnemies = 0;

    private void Update()
    {
        if (player == null || enemyPrefab == null) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval && currentEnemies < maxEnemies)
        {
            SpawnHorizontalEnemy();
            timer = 0f;
        }
    }

    private void SpawnHorizontalEnemy()
    {
        // Decide si aparece a la izquierda (-1) o derecha (+1)
        float direction = Random.value < 0.5f ? -1f : 1f;

        // Posición de aparición
        Vector2 spawnPos = new Vector2(
            player.position.x + (horizontalDistance * direction),
            player.position.y + Random.Range(-verticalVariation, verticalVariation)
        );

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemies++;

  
    }

}
