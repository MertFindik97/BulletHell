using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int contactDamage = 1;
    [SerializeField] float damageCooldown = 1f;

    Rigidbody2D rb;
    Transform target;
    float damageTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
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
}
