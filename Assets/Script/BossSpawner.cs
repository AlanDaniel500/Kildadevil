using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject bossPrefab;
    public static BossSpawner Instance;

    void Awake() => Instance = this;

    public void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
    }
}
