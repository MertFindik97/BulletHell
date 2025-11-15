using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 2f;
    [SerializeField] public int contactDamage = 1;
    [SerializeField] public float damageCooldown = 1f;

    Rigidbody2D rb;
    Transform target;
    Animator anim;

    float damageTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();     // 🔥 Animator holen
        target = GameObject.FindWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        if (target == null) return;

    Vector2 direction = (target.position - transform.position).normalized;
    Vector2 newPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

    Vector2 velocity = (newPos - rb.position) / Time.fixedDeltaTime;

    rb.MovePosition(newPos);

    anim.SetFloat("Speed", velocity.magnitude);

    // Gegner richtig drehen – REVERSE FLIP
    if (direction.x > 0.01f)
    transform.localScale = new Vector3(-1, 1, 1);  // Kopf nach rechts
    else if (direction.x < -0.01f)
    transform.localScale = new Vector3(1, 1, 1);   // Kopf nach links

    }

    void Update()
    {
        damageTimer -= Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (damageTimer > 0f) return;
        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent<Health>(out var hp))
        {
            hp.TakeDamage(contactDamage);
            damageTimer = damageCooldown;
        }
    }

    public void Die()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddXP(20);
            Debug.Log("💥 Gegner tot – XP vergeben!");
        }
        else
        {
            Debug.LogWarning("⚠ Kein aktiver PlayerStats – Gegner stirbt ohne XP-Vergabe.");
        }

        ScoreManager.Instance?.AddPoints(100);
        Destroy(gameObject);
    }
}
