using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] Health health;

    void Awake()
    {
        if (health == null)
            health = GetComponentInParent<Health>();
    }

    void Update()
    {
        if (health == null || fillImage == null) return;

        float ratio = (float)health.CurrentHP / health.MaxHP;
        fillImage.fillAmount = ratio;
        transform.rotation = Quaternion.identity; // bleibt flach zur Kamera
    }
}
