using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Schuss-Einstellungen")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float defaultShotsPerSecond = 0.5f;

    [Header("Audio")]
    [SerializeField] AudioClip shootClip; // 🔊 dein Feuerball-Sound

    Animator anim;
    AudioSource audioSource;
    float cooldown;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>(); // 🔊 AudioSource holen
    }

    void Update()
    {
        cooldown -= Time.deltaTime;

        if (Input.GetMouseButton(0))
            TryFire();
    }

    void TryFire()
    {
        if (cooldown > 0f) return;

        float shotsPerSecond = PlayerStats.Instance != null
            ? PlayerStats.Instance.shotsPerSecond
            : defaultShotsPerSecond;

        cooldown = 1f / shotsPerSecond;

        // Animation
        if (anim != null)
            anim.SetTrigger("ShootTrigger");

        // Projektil
        var bullet = BulletPool.Instance.Get(bulletPrefab);
        bullet.Launch(firePoint.up, firePoint.position);

        // 🔊 Sound abspielen
        if (shootClip != null && audioSource != null)
            audioSource.PlayOneShot(shootClip);
    }
}
