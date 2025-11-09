using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private GameObject upgradeCanvas;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Menü beim Start ausblenden
        if (upgradeCanvas != null)
            upgradeCanvas.SetActive(false);
    }

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

    public void OnUpgradeHealth()
    {
        PlayerStats.Instance?.UpgradeHealth();
        CloseUpgradeMenu();
    }

    public void OnUpgradeSpeed()
    {
        PlayerStats.Instance?.UpgradeSpeed();
        CloseUpgradeMenu();
    }

    public void OnUpgradeFireRate()
    {
        PlayerStats.Instance?.UpgradeFireRate();
        CloseUpgradeMenu();
    }
}
