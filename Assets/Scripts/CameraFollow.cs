using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // dein Spieler
    [SerializeField] private float smoothSpeed = 0.125f; // wie weich die Kamera folgt
    [SerializeField] private Vector3 offset; // Abstand zur Spielfigur

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
