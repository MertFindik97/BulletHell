using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;// das sind die Projektiele
    [SerializeField] Transform firePoint;// das sind die Schüsse 
    [SerializeField] float shotsPerSecond = 6f;// schüsse pro sekunde

    float cooldown;// das es nicht wie eine alles durschgehent schießt sondern mit pausen

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (Input.GetMouseButton(0)) // Linke Maustaste gedrückt
            TryFire();
    }

    void TryFire()
    {
     if (cooldown > 0f) return;

     // Neue Zeile: FireRate dynamisch aus PlayerStats
     float currentFireRate = PlayerStats.Instance != null ? PlayerStats.Instance.fireRate : 1f;

        cooldown = currentFireRate; // fireRate aus PlayerStats = Sekunden zwischen Schüssen

      var bullet = BulletPool.Instance.Get(bulletPrefab);
        bullet.Launch(firePoint.up, firePoint.position);
    }

}
