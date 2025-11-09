using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP = 3; // Grund-Leben (z. B. Gegner)
    public UnityEvent onDeath;

    int current;

    public int CurrentHP => current;
    public int MaxHP => maxHP;

    void Awake()
    {
        // Wenn es der Player ist → Leben aus PlayerStats übernehmen
        if (CompareTag("Player") && PlayerStats.Instance != null)
        {
            maxHP = PlayerStats.Instance.maxHealth;
        }

        // Fallback, falls maxHP versehentlich 0 ist
        if (maxHP <= 0)
        {
            maxHP = 3;
            Debug.LogWarning($"{gameObject.name} hatte kein gültiges maxHP, Standardwert 3 gesetzt!");
        }

        current = maxHP;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log($"{gameObject.name} took {amount} damage!");
        current -= amount;

        if (current <= 0)
        {
            // Punkte vergeben, wenn Gegner stirbt
            if (CompareTag("Enemy"))
            {
                ScoreManager.Instance?.AddPoints(100);
            }

            onDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Setzt ein neues maximales Leben (z. B. nach Upgrade)
    /// </summary>
    public void SetMaxHP(int newMax)
    {
        maxHP = newMax;
        current = maxHP;
        Debug.Log($"❤️ Neues maximales Leben: {maxHP}");
    }

    /// <summary>
    /// Heilt auf volle Lebenspunkte
    /// </summary>
    public void HealToFull()
    {
        if (CompareTag("Player") && PlayerStats.Instance != null)
        {
            maxHP = PlayerStats.Instance.maxHealth;
        }

        current = Mathf.Max(1, maxHP);
        Debug.Log($"💗 Volles Leben wiederhergestellt ({current}/{maxHP})");
    }
}
