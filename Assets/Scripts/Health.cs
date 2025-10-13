using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHP = 3;// Lebensanzeige (Wie viel Herzen hat man)
    public UnityEvent onDeath;// wenn man kein Herz mehr hat
    int current;// was er grad vorhanden hat an Herzen

    void Awake() => current = maxHP;//Start mit der Menge an herzen am anfang des Spieles

    public void TakeDamage(int amount) // nimmt der Player schaden wird hier bestimmt wie viel ernocht hat und wie viel schaden er nimmt
    {
        current -= amount;
        if (current <= 0)
        {
            onDeath?.Invoke();//wenn man kein leben mehr hat is das Spiel fertig 
            gameObject.SetActive(false);
        }
    }
}
