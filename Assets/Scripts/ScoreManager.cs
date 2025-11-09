using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;      // aktueller Score im Spiel
    [SerializeField] private TextMeshProUGUI highscoreText;  // Highscore (z.B. im GameOver-UI)

    private int score;
    private int highscore;

    void Awake()
    {
        Instance = this;

        // Highscore aus PlayerPrefs laden
        highscore = PlayerPrefs.GetInt("Highscore", 0);

        // Szene-Event abonnieren
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Beim Laden einer neuen Szene UI-Referenzen neu finden
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();

        if (highscoreText == null)
            highscoreText = GameObject.Find("HighScoreText")?.GetComponent<TextMeshProUGUI>();

        // Wenn du wieder in die Spielszene kommst, Score auf 0 setzen
        if (scene.name == "Main") // <-- anpassen an deine Spielszene
            ResetScore();

        UpdateUI();
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    public void SaveHighscore()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save();
            Debug.Log($"✅ Neuer Highscore gespeichert: {highscore}");
        }
        else
        {
            Debug.Log($"ℹ Kein neuer Highscore. Aktuell: {score} / Bester: {highscore}");
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";

        if (highscoreText != null)
            highscoreText.text = $"Highscore: {highscore}";
    }

    public int CurrentScore => score;
    public int Highscore => highscore;
}
