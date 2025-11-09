using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Schuss-Einstellungen")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float defaultShotsPerSecond = 0.5f;

    float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;

        if (Input.GetMouseButton(0)) // Linke Maustaste gedrückt
            TryFire();
    }

    void TryFire()
    {
        if (cooldown > 0f) return;

        // 🔹 FireRate dynamisch aus PlayerStats holen
        float shotsPerSecond = PlayerStats.Instance != null 
            ? PlayerStats.Instance.shotsPerSecond 
            : defaultShotsPerSecond;

        cooldown = 1f / shotsPerSecond; // Sekunden zwischen Schüssen

        var bullet = BulletPool.Instance.Get(bulletPrefab);
        bullet.Launch(firePoint.up, firePoint.position);
    }
}
