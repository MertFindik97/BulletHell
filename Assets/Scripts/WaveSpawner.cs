using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveEnemy
{
    public GameObject prefab;
    public int amount;
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public WaveEnemy[] enemies;
    public float rate;
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 4f;

    [Header("Boss Settings")]
    public GameObject[] bossPrefabs;        // mehrere Bosse möglich
    public int bossWaveInterval = 5;

    public float bossHpMultiplier = 1.2f;    // +20% HP pro Boss-Wave
    public float bossDamageMultiplier = 1.1f; // +10% Damage pro Boss-Wave
    public float bossSpeedMultiplier = 1.1f;  // +10% Speed pro Boss-Wave

    [Header("Endless Mode")]
    public bool endlessMode = true;

    [Header("Player Reference")]
    public Transform player;
    public float minSpawnDistance = 3f;

    [Header("UI")]
    public TextMeshProUGUI waveAnnouncementText;
    public TextMeshProUGUI waveCounterText;

    int currentWaveIndex = 0;
    int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(HandleWaves());
    }

    IEnumerator HandleWaves()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            // Prüfen ob Boss-Wave
            bool isBossWave = (currentWaveIndex + 1) % bossWaveInterval == 0;

            // BOSSWAVE
            if (isBossWave)
            {
                UpdateWaveUI("BOSS WAVE!");
                yield return StartCoroutine(ShowBossAnnouncement());

                SpawnBossWave();

                while (enemiesAlive > 0)
                    yield return null;

                yield return new WaitForSeconds(timeBetweenWaves);
                currentWaveIndex++;
                continue;
            }

            // NORMALE WAVE
            Wave wave;

            bool isEndless = currentWaveIndex >= waves.Length;

            if (!isEndless)
            {
                wave = waves[currentWaveIndex];
            }
            else
            {
                wave = GenerateEndlessWave(currentWaveIndex);
            }

            UpdateWaveUI("Wave " + (currentWaveIndex + 1));
            yield return StartCoroutine(ShowWaveAnnouncement());

            yield return StartCoroutine(SpawnWave(wave));

            while (enemiesAlive > 0)
                yield return null;

            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
        }
    }

    // -------------------------------------------------------------------
    // BOSS WAVES
    // -------------------------------------------------------------------

    void SpawnBossWave()
    {
        int bossWaveNumber = (currentWaveIndex + 1) / bossWaveInterval;

        foreach (var bossPrefab in bossPrefabs)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject boss = Instantiate(bossPrefab, point.position, Quaternion.identity);

            // Boss skalieren
            ScaleBossStats(boss, bossWaveNumber);

            // Tod registrieren
            var notifier = boss.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;

            enemiesAlive++;
        }
    }

    void ScaleBossStats(GameObject boss, int bossWaveNumber)
    {
        // HP skalieren
        if (boss.TryGetComponent<Health>(out var hp))
        {
            int scaledHP = Mathf.RoundToInt(hp.MaxHP * Mathf.Pow(bossHpMultiplier, bossWaveNumber));
            hp.SetMaxHP(scaledHP);
        }

        // Speed + Damage skalieren
        if (boss.TryGetComponent<EnemyAI>(out var ai))
        {
            ai.moveSpeed *= Mathf.Pow(bossSpeedMultiplier, bossWaveNumber);
            ai.contactDamage = Mathf.RoundToInt(ai.contactDamage * Mathf.Pow(bossDamageMultiplier, bossWaveNumber));
        }
    }

    IEnumerator ShowBossAnnouncement()
    {
        waveAnnouncementText.text = "⚠️ BOSS WAVE!";
        waveAnnouncementText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        waveAnnouncementText.gameObject.SetActive(false);
    }

    // -------------------------------------------------------------------
    // NORMALE WAVES
    // -------------------------------------------------------------------

    IEnumerator SpawnWave(Wave wave)
    {
        // Liste aller Gegner in der Wave erstellen
        List<GameObject> list = new List<GameObject>();

        foreach (WaveEnemy we in wave.enemies)
        {
            for (int i = 0; i < we.amount; i++)
                list.Add(we.prefab);
        }

        // Mischen
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }

        // Gegner spawnen
        foreach (var prefab in list)
        {
            Transform point = GetValidSpawnPoint();

            GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);

            EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;

            enemiesAlive++;
            yield return new WaitForSeconds(1f / wave.rate);
        }
    }

    // -------------------------------------------------------------------
    // ENDLOSE WAVES
    // -------------------------------------------------------------------

    Wave GenerateEndlessWave(int index)
    {
        Wave w = new Wave();
        w.waveName = "Endless " + (index + 1);
        w.rate = 1.2f + index * 0.05f;

        Wave baseWave = waves[waves.Length - 1];

        w.enemies = new WaveEnemy[baseWave.enemies.Length];

        for (int i = 0; i < baseWave.enemies.Length; i++)
        {
            w.enemies[i] = new WaveEnemy
            {
                prefab = baseWave.enemies[i].prefab,
                amount = baseWave.enemies[i].amount + index * 2
            };
        }

        return w;
    }

    // -------------------------------------------------------------------
    // HELPER
    // -------------------------------------------------------------------

    Transform GetValidSpawnPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];

            if (player == null) return p;

            if (Vector2.Distance(p.position, player.position) >= minSpawnDistance)
                return p;
        }

        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    public void OnEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;
    }

    void UpdateWaveUI(string text)
    {
        if (waveCounterText != null)
            waveCounterText.text = text;
    }

    IEnumerator ShowWaveAnnouncement()
    {
        waveAnnouncementText.text = $"Wave {currentWaveIndex + 1} beginnt!";
        waveAnnouncementText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        waveAnnouncementText.gameObject.SetActive(false);
    }
}
