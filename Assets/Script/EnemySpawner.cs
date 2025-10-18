using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public Transform leftSpawn, rightSpawn;

    private float timer;
    private float elapsedTime;

    void Update()
    {
        if (GameManager.Instance.IsPaused) return;

        timer += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
            spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.01f);
        }
    }

    void SpawnEnemy()
    {
        Transform side = Random.value > 0.5f ? leftSpawn : rightSpawn;
        var e = Instantiate(enemyPrefab, side.position, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.maxHealth += elapsedTime * 0.2f;
    }

}
