using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;          // Singleton, damit es nur eine Instanz gibt
    public static bool GameIsPaused;

    [SerializeField] GameObject pauseMenuUI;   // wird automatisch gefunden, falls leer

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // bleibt über Szenenwechsel bestehen

        Time.timeScale = 1f;
        GameIsPaused = false;

        // Erstes Rebind (falls Szene schon die UI hat)
        TryRebindPauseMenuUI();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Bei jeder neuen Szene sicherstellen, dass wir weiterlaufen & UI neu binden
        Time.timeScale = 1f;
        GameIsPaused = false;
        TryRebindPauseMenuUI();
    }

    void TryRebindPauseMenuUI()
    {
        if (pauseMenuUI != null) return;

        // 1) per Tag suchen (empfohlen)
        var tagged = GameObject.FindWithTag("PauseMenu");
        if (tagged != null)
        {
            pauseMenuUI = tagged;
            pauseMenuUI.SetActive(false); // sicherheitshalber
            return;
        }

        // 2) Fallback per Name
        var byName = GameObject.Find("PauseMenu");
        if (byName != null)
        {
            pauseMenuUI = byName;
            pauseMenuUI.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI == null) { Debug.LogWarning("PauseMenuUI is missing!"); return; }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        if (pauseMenuUI == null) { Debug.LogWarning("PauseMenuUI is missing!"); return; }
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
