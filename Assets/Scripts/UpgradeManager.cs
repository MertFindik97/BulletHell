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
    }

    public void OpenUpgradeMenu()
    {
        Time.timeScale = 0f; // Spiel pausieren
        upgradeCanvas.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        Time.timeScale = 1f; // Weiter spielen
        upgradeCanvas.SetActive(false);
    }

    public void OnUpgradeHealth()
    {
        PlayerStats.Instance.UpgradeHealth();
        CloseUpgradeMenu();
    }

    public void OnUpgradeSpeed()
    {
        PlayerStats.Instance.UpgradeSpeed();
        CloseUpgradeMenu();
    }

    public void OnUpgradeFireRate()
    {
        PlayerStats.Instance.UpgradeFireRate();
        CloseUpgradeMenu();
    }
}
