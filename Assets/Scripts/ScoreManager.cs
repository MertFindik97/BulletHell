using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highscoreText; // ⬅ für Game Over Anzeige
    int score;
    int highscore;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Highscore aus PlayerPrefs laden
        highscore = PlayerPrefs.GetInt("Highscore", 0);
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
            Debug.Log("✅ Neuer Highscore gespeichert: " + highscore);
        }else
    {
        Debug.Log("ℹ Kein neuer Highscore. Aktuell: " + score + " / Bester: " + highscore);
    }
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

internal class puplic
{
}