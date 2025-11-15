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
    public float timeBetweenWaves = 4f;

    [Header("Boss Settings")]
    public GameObject[] bossPrefabs;
    public int bossWaveInterval = 5;

    public float bossHpMultiplier = 1.2f;
    public float bossDamageMultiplier = 1.1f;
    public float bossSpeedMultiplier = 1.1f;

    [Header("Endless Mode")]
    public bool endlessMode = true;

    [Header("Player")]
    public Transform player;

    [Header("Spawn Settings")]
    public float minSpawnDistance = 5f;     // Abstand zum Spieler
    public float maxSpawnDistance = 9f;     // Kreisradius 0

    [Header("UI")]
    public TextMeshProUGUI waveAnnouncementText;
    public TextMeshProUGUI waveCounterText;

    int currentWaveIndex = 0;
    int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(HandleWaves());
    }

    // -------------------------------------------------------------
    // WAVE HANDLING
    // -------------------------------------------------------------
    IEnumerator HandleWaves()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            bool isBossWave = (currentWaveIndex + 1) % bossWaveInterval == 0;

            if (isBossWave)
            {
                UpdateWaveUI("BOSS WAVE!");
                yield return ShowBossAnnouncement();

                SpawnBossWave();

                while (enemiesAlive > 0)
                    yield return null;

                yield return new WaitForSeconds(timeBetweenWaves);
                currentWaveIndex++;
                continue;
            }

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
            yield return ShowWaveAnnouncement();

            yield return SpawnWave(wave);

            while (enemiesAlive > 0)
                yield return null;

            yield return new WaitForSeconds(timeBetweenWaves);
            currentWaveIndex++;
        }
    }

    // -------------------------------------------------------------
    // BOSS WAVES
    // -------------------------------------------------------------

    void SpawnBossWave()
    {
        int bossWaveNumber = (currentWaveIndex + 1) / bossWaveInterval;

        foreach (var bossPrefab in bossPrefabs)
        {
            Vector3 pos = GetRandomSpawnPosition();
            GameObject boss = Instantiate(bossPrefab, pos, Quaternion.identity);

            ScaleBossStats(boss, bossWaveNumber);

            var notifier = boss.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;

            enemiesAlive++;
        }
    }

    void ScaleBossStats(GameObject boss, int bossWaveNumber)
    {
        if (boss.TryGetComponent<Health>(out var hp))
        {
            int scaledHP = Mathf.RoundToInt(hp.MaxHP * Mathf.Pow(bossHpMultiplier, bossWaveNumber));
            hp.SetMaxHP(scaledHP);
        }

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

    // -------------------------------------------------------------
    // NORMAL WAVES
    // -------------------------------------------------------------

    IEnumerator SpawnWave(Wave wave)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (WaveEnemy we in wave.enemies)
        {
            for (int i = 0; i < we.amount; i++)
                list.Add(we.prefab);
        }

        // Shuffle
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }

        foreach (var prefab in list)
        {
            Vector3 pos = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);

            EnemyDeathNotifier notifier = enemy.AddComponent<EnemyDeathNotifier>();
            notifier.spawner = this;

            enemiesAlive++;

            yield return new WaitForSeconds(1f / wave.rate);
        }
    }

    // -------------------------------------------------------------
    // ENDLESS MODE
    // -------------------------------------------------------------

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

    // -------------------------------------------------------------
    // SPAWN POSITION CALCULATION
    // -------------------------------------------------------------

    Vector3 GetRandomSpawnPosition()
    {
        if (player == null)
            return Vector3.zero;

        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

        return player.position + (Vector3)offset;
    }

    // -------------------------------------------------------------
    // EVENTS
    // -------------------------------------------------------------

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
