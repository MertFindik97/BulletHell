using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemyAI enemyPrefab;
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] float spawnRadius = 8f;

    float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = spawnInterval;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
