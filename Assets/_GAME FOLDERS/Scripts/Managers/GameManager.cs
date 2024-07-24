using UnityEngine;

public class GameManager : SingleReference<GameManager>
{
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

    public bool IsGamePaused()
    {
        return isPaused;
    }
}
