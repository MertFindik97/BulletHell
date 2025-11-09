using UnityEngine;

public class EnemyDeathNotifier : MonoBehaviour
{
    public WaveSpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDeath();
        }
    }
}
