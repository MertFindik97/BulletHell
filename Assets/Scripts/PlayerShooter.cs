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
        cooldown = 1f / shotsPerSecond;

        var bullet = BulletPool.Instance.Get(bulletPrefab);
        //var bullet = Instantiate(bulletPrefab); Bullet funktinieren aber werden nicht von BulletPool gesteuert
        // das macht es etwas Langsame und unevizienter

        bullet.Launch(firePoint.up, firePoint.position); // Wichtig: firePoint.up = Schussrichtung 
        // 
    }
}
