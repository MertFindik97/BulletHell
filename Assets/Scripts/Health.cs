using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP = 3; // Lebensanzeige (Wie viel Herzen hat man(3))
    public UnityEvent onDeath; // wenn man kein Herz mehr hat
    int current; // was er grad vorhanden hat an Herzen

    public int CurrentHP => current;
    public int MaxHP => maxHP;

    void Awake() => current = maxHP; // Start mit der Menge an Herzen am Anfang des Spieles

    public void TakeDamage(int amount)
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
