using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyElitePrefab;
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
        GameObject type = Random.value > 0.05f ? enemyPrefab : enemyElitePrefab;
        var e = Instantiate(type, new Vector3(side.position.x, side.position.y + Random.Range(-3f,+3f), side.position.z), Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();
        int randomNumber = Random.Range(1, 101);
        if (randomNumber <= 50)
        {
            enemy.Initialize(1);
        }
        else
        {
            if (randomNumber > 50 && randomNumber <= 80)
            {
                enemy.Initialize(2);
            }
            else
            {
                enemy.Initialize(3);
            }
        }
        enemy.maxHealth += elapsedTime * 0.2f;
    }

}
