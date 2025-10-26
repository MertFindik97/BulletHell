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
        // Sobald Gegner stirbt, Bar ausblenden
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        float duration = 0.5f;
        float startAlpha = canvasGroup.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / duration);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
