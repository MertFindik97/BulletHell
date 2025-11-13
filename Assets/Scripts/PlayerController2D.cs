using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;

    Rigidbody2D rb;
    Vector2 input;
    Camera cam;
    Animator anim;

    [Header("References")]
    public Transform firePoint; // zum Drehen des Schusses

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // WASD Bewegung
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = Vector2.ClampMagnitude(input, 1f);

        // Animation setzen
        anim.SetFloat("Speed", input.magnitude);

        // Maus-Tracking → NUR FirePoint dreht sich!
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDir = (mousePos - firePoint.position);
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * PlayerStats.Instance.moveSpeed;
    }
}
