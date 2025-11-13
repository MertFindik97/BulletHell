using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Schuss-Einstellungen")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float defaultShotsPerSecond = 0.5f;

    Animator anim;
    float cooldown;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        cooldown -= Time.deltaTime;

        if (Input.GetMouseButton(0)) // Linke Maustaste gedrückt
            TryFire();
    }

    void TryFire()
    {
        if (cooldown > 0f) return;

        // FireRate aus PlayerStats oder Fallback
        float shotsPerSecond = PlayerStats.Instance != null
            ? PlayerStats.Instance.shotsPerSecond
            : defaultShotsPerSecond;

        cooldown = 1f / shotsPerSecond;

        // 🔥 Attack-Animation auslösen
        if (anim != null)
            anim.SetTrigger("ShootTrigger");

        // 🔥 Projektil abfeuern
        var bullet = BulletPool.Instance.Get(bulletPrefab);
        bullet.Launch(firePoint.up, firePoint.position);
    }
}
