using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    public int maxHealth = 100;
    public float moveSpeed = 10f;
    public float fireRate = 5f;

    void Awake()
    {
        Instance = this;
        Debug.Log("✅ PlayerStats initialisiert.");
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"XP erhalten: {amount} | Aktuell: {currentXP}/{xpToNextLevel}");

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
         level++;
    currentXP -= xpToNextLevel;
    xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);
    Debug.Log($"🆙 Level {level} erreicht!");

    if (UpgradeManager.Instance != null)
        UpgradeManager.Instance.OpenUpgradeMenu();
    else
        Debug.LogError("❌ UpgradeManager.Instance ist null!");
    }

    public void UpgradeHealth()  { maxHealth += 1; Debug.Log("💗 Leben erhöht!"); }
    public void UpgradeSpeed()   { moveSpeed += 1f; Debug.Log("⚡ Geschwindigkeit erhöht!"); }
    public void UpgradeFireRate(){ fireRate *= 0.9f; Debug.Log("🔫 Schussrate erhöht!"); }
}
