using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverCanvas;
    bool isGameOver = false;

    void Awake()
    {
        Time.timeScale = 1f; // immer normal starten
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
        isGameOver = false;
    }

    public void ShowGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        Time.timeScale = 0f; // Spiel anhalten
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main"); //  nur Main neu laden!
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); //  nur hier MainMenu laden
    }
}
