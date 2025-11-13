using TMPro;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Wave
{
    public string waveName;
    public GameObject[] enemyPrefabs;
    public int count;
    public float rate;
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;

    [Header("Player Reference")]
    public Transform player;            // Player-Objekt
    public float minSpawnDistance = 3f; // Mindestabstand beim Spawnen

    [Header("UI References")]
    public TextMeshProUGUI waveAnnouncementText;
    public TextMeshProUGUI waveCounterText;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(HandleWaves());
    }

    IEnumerator HandleWaves()
    {
        yield return new WaitForSeconds(2f); // kleiner Start-Delay

        while (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];
            UpdateWaveUI();
            yield return StartCoroutine(ShowWaveAnnouncement());

            // Starte Wave
            yield return StartCoroutine(SpawnWave(wave));

            // Warte bis alle Gegner tot sind
            while (enemiesAlive > 0)
                yield return null;

            // Pause zwischen Waves
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
        }

        Debug.Log("🎉 Alle Waves besiegt!");
        waveAnnouncementText.text = "🎉 Alle Waves besiegt!";
        waveAnnouncementText.gameObject.SetActive(true);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;

        for (int i = 0; i < wave.count; i++)
        {
            GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
            Transform spawnPoint = GetValidSpawnPoint();

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemiesAlive++;

            // Gegner wird registriert, damit der Spawner weiß, wann er tot ist
            EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;

            yield return new WaitForSeconds(1f / wave.rate);
        }

        isSpawning = false;
    }

    Transform GetValidSpawnPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            if (player == null) return point;

            float distance = Vector2.Distance(point.position, player.position);
            if (distance >= minSpawnDistance)
                return point;
        }

        // Falls kein passender Punkt gefunden wurde (sollte selten vorkommen)
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    public void OnEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;
    }

    void UpdateWaveUI()
    {
        if (waveCounterText != null)
            waveCounterText.text = "Wave: " + (currentWaveIndex + 1);
    }

    IEnumerator ShowWaveAnnouncement()
    {
        if (waveAnnouncementText == null) yield break;

        waveAnnouncementText.text = "Wave " + (currentWaveIndex + 1) + " beginnt!";
        waveAnnouncementText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        waveAnnouncementText.gameObject.SetActive(false);
    }
}
