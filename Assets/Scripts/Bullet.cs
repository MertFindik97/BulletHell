using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] int damage = 1;
    [SerializeField] float lifeTime = 1f;

    Rigidbody2D rb;
    float life;//

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, Vector3 startPosition)
    {
        //Debug.Log("Launch called!");// gucken ob diese Methode Launch oder igniert wurde denn Schuss bleibt auf der Stelle
        // oder Spawnt an einem Fixen punkt
        transform.position = startPosition; // Startposition auf FirePoint setzen
        life = lifeTime;
        rb.linearVelocity = direction.normalized * speed;

        gameObject.SetActive(true);
    }


    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0f)
            ReturnToPool();
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.TryGetComponent<Health>(out var hp))
    {
        hp.TakeDamage(damage);
        ReturnToPool();
    }
}

    

    void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
