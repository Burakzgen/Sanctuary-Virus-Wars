using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleReference<GameManager>
{
    [SerializeField] private GameObject gameOverPanel;
    private bool isPaused = false;
    [SerializeField] FirstPersonMovement m_firstPersonMovement;
    public void PauseGame()
    {
        if (isPaused) return;

        m_firstPersonMovement.IsPause = true;
        isPaused = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        // Diðer durdurma iþlemleri eklenecek
        Debug.Log("Game Paused");
    }
    public void GameOver()
    {
        m_firstPersonMovement.IsPause = true;
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        gameOverPanel.SetActive(true);
        Debug.Log("Game Over");
    }
    public void ContinueGame()
    {
        if (!isPaused) return;

        m_firstPersonMovement.IsPause = false;
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Game Continued");
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public bool IsGamePaused()
    {
        return isPaused;
    }
}
