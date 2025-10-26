using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] Health health;
    [SerializeField] CanvasGroup canvasGroup;

    void Awake()
    {
        if (health == null)
            health = GetComponentInParent<Health>();
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // Wenn das Health-Script das UnityEvent „onDeath“ hat → abonnieren
        if (health != null)
            health.onDeath.AddListener(OnDeath);
    }

    void Update()
    {
        if (health == null || fillImage == null) return;

        float ratio = (float)health.CurrentHP / health.MaxHP;
        fillImage.fillAmount = ratio;

        // Immer zur Kamera drehen (optional)
        transform.rotation = Quaternion.identity;
    }

    void OnDeath()
    {
        Debug.Log($"{gameObject.name} took {amount} damage!");
        current -= amount;

        if (current <= 0)
        {
            // Punkte vergeben, wenn es ein Gegner ist
            if (CompareTag("Enemy"))
            {
                ScoreManager.Instance?.AddPoints(100);
            }

            onDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }

}
