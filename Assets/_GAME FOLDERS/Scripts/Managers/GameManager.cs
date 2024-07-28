using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleReference<GameManager>
{
    [SerializeField] GameObject weapon1, weapon2;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI zombieKillCountText;
    private bool isPaused = false;

    [Header("Character Referance")]
    [SerializeField] FirstPersonMovement m_firstPersonMovement;

    private void Start()
    {
        UpdateButtonVisibility();

        PlayerPrefsManager.ResetZombieKillCount();
        zombieKillCountText.text = PlayerPrefsManager.ZombieKillCount.ToString();
    }
    public void UpdateZombieCountUI()
    {
        PlayerPrefsManager.IncrementZombieKillCount();
        zombieKillCountText.text = PlayerPrefsManager.ZombieKillCount.ToString();
        Debug.Log("High Zombie Kill :" + PlayerPrefsManager.HighestZombieKillCount);
    }
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
        PauseChracterControls();
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        Debug.Log("Game Over");
    }
    public void ContinueGame()
    {
        if (!isPaused) return;

        ResumeChracterControls();
        Time.timeScale = 1f;
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
    public void UpdateButtonVisibility()
    {
        weapon1.SetActive(IsItemPurchased(weapon1.name));
        weapon2.SetActive(IsItemPurchased(weapon2.name));
    }
    public bool IsItemPurchased(string itemName)
    {
        return PlayerPrefs.GetInt(itemName, 0) == 1;
    }
    public void PauseChracterControls(bool cursorLock = true)
    {
        m_firstPersonMovement.IsPause = true;
        isPaused = true;
        if (cursorLock)
            Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeChracterControls(bool cursorLock = true)
    {
        m_firstPersonMovement.IsPause = false;
        isPaused = false;
        if (cursorLock)
            Cursor.lockState = CursorLockMode.Locked;
    }
}
