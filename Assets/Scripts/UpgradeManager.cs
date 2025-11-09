using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private GameObject upgradeCanvas;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void OpenUpgradeMenu()
    {
        Time.timeScale = 0f;
        upgradeCanvas.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        Time.timeScale = 1f;
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
