using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private GameObject upgradeCanvas;

    private Health playerHealth;

    void Awake()
    {
        // Singleton-Setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("✅ UpgradeManager Instance gesetzt: " + gameObject.name);

        if (upgradeCanvas != null)
            upgradeCanvas.SetActive(false);
        else
            Debug.LogError("❌ UpgradeCanvas fehlt im Inspector!");
    }

    void Start()
    {
        // Spieler-Health finden
        playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Health>();

        if (playerHealth == null)
            Debug.LogWarning("⚠️ Player-Health konnte nicht gefunden werden!");
        else
            Debug.Log("✅ Player-Health erfolgreich gefunden.");
    }

    // -----------------------------
    // Menüsteuerung
    // -----------------------------

    public void OpenUpgradeMenu()
    {
        if (upgradeCanvas == null)
        {
            Debug.LogError("❌ UpgradeCanvas ist im Inspector nicht zugewiesen!");
            return;
        }

        Debug.Log("🧙‍♂️ Upgrade-Menü wird geöffnet...");
        Time.timeScale = 0f;
        upgradeCanvas.SetActive(true);
        Debug.Log($"✅ Canvas aktiv: {upgradeCanvas.activeSelf}");
    }

    public void CloseUpgradeMenu()
    {
        if (upgradeCanvas == null) return;
        Time.timeScale = 1f;
        upgradeCanvas.SetActive(false);
    }

    // -----------------------------
    // Upgrades
    // -----------------------------

    public void OnUpgradeHealth()
    {
        PlayerStats.Instance?.UpgradeHealth();

        if (playerHealth != null && playerHealth.CompareTag("Player"))
        {
            playerHealth.SetMaxHP(PlayerStats.Instance.maxHealth);
            playerHealth.HealToFull();
        }

        LogCurrentStats();
        CloseUpgradeMenu();
    }

    public void OnUpgradeSpeed()
    {
        PlayerStats.Instance?.UpgradeSpeed();
        LogCurrentStats();
        CloseUpgradeMenu();
    }

    public void OnUpgradeFireRate()
    {
        PlayerStats.Instance?.UpgradeFireRate();
        LogCurrentStats();
        CloseUpgradeMenu();
    }

    // -----------------------------
    // Anzeige der aktuellen Werte
    // -----------------------------

    private void LogCurrentStats()
    {
       var stats = PlayerStats.Instance;
    if (stats == null) return;

    string text =
        $"📊 AKTUELLE WERTE ({System.DateTime.Now:HH:mm:ss}):\n" +
        $"💗 Leben: {stats.maxHealth}\n" +
        $"⚡ Geschwindigkeit: {stats.moveSpeed:F1}\n" +
        $"🔫 Schussrate: {stats.shotsPerSecond:F1} Schüsse/Sekunde";

    Debug.Log(text);
    }
}
