using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wave
{
    public string waveName;
    public GameObject[] enemyPrefabs; // verschiedene Gegnertypen
    public int count; // wie viele Gegner
    public float rate; // wie schnell sie erscheinen
}

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    private int currentWaveIndex = 0;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2f); // Start-Delay

        while (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];
            Debug.Log("Spawning Wave: " + wave.waveName);
            yield return StartCoroutine(SpawnWave(wave));
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWaveIndex++;
        }

        Debug.Log("🎉 Alle Waves besiegt!");
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;

        for (int i = 0; i < wave.count; i++)
        {
            GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(1f / wave.rate);
        }

        isSpawning = false;
    }
}
