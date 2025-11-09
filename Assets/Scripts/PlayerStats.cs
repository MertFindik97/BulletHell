using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Level-System")]
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    [Header("Spielerwerte")]
    public int maxHealth = 3;
    public float moveSpeed = 1f;
    public float shotsPerSecond = 1f; // intuitiver als fireRate in Sekunden

    void Awake()
    {
        Instance = this;
        Debug.Log("✅ PlayerStats initialisiert.");
    }

    // -----------------------------
    // XP-System
    // -----------------------------
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

    // -----------------------------
    // Upgrades
    // -----------------------------

    public void UpgradeHealth()
    {
        maxHealth += 1;
        Debug.Log($"💗 Leben erhöht! Neues Max-Leben: {maxHealth}");
    }

    public void UpgradeSpeed()
    {
        moveSpeed += 1f;
        Debug.Log($"⚡ Geschwindigkeit erhöht! Neue Geschwindigkeit: {moveSpeed:F1}");
    }

    public void UpgradeFireRate()
    {
        shotsPerSecond += 1f;
        Debug.Log($"🔫 Schussrate erhöht! Neue Schüsse/Sekunde: {shotsPerSecond:F1}");
    }
}
