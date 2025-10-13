using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;

    Rigidbody2D rb;
    Vector2 input;
    Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        // Bewegung (WASD)
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = Vector2.ClampMagnitude(input, 1f);

        // Richtung zur Maus berechnen
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDir = (mousePos - transform.position);
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;

        // Rotation setzen
        rb.MoveRotation(angle);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * moveSpeed;
    }
}
