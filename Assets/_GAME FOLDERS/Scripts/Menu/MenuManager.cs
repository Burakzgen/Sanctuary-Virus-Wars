using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button controllerButton;
    [SerializeField] private Button closeLeaderboardButton;
    [SerializeField] private Button closeCreditsButton;
    [SerializeField] private Button closeControllerButton;
    [SerializeField] private Button qualityNextButton;
    [SerializeField] private Button qualityPreviousButton;
    [SerializeField] private Button submitNicknameButton;

    [Header("Sprites")]
    [SerializeField] private Sprite newGameSprite;
    [SerializeField] private Sprite quitSprite;

    [Header("Sliders")]
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField nicknameInputField;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI nickNameErrorText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI cameraSensitivityText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI uiVolumeText;

    [Header("Panels")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject controllerPanel;
    [SerializeField] private GameObject inputPanel;

    [Header("Popup Manager")]
    [SerializeField] private PopupManager popupManager;

    private int currentQualityLevel;

    private void Start()
    {
        // Buttons
        newGameButton.onClick.AddListener(OnNewGameClicked);
        quitGameButton.onClick.AddListener(OnQuitGameClicked);
        leaderboardButton.onClick.AddListener(OpenLeaderboardPanel);
        creditsButton.onClick.AddListener(OpenCreditsPanel);
        controllerButton.onClick.AddListener(OpenControllerPanel);
        closeLeaderboardButton.onClick.AddListener(CloseLeaderboardPanel);
        closeCreditsButton.onClick.AddListener(CloseCreditsPanel);
        closeControllerButton.onClick.AddListener(CloseControllerPanel);
        qualityNextButton.onClick.AddListener(OnQualityNextClicked);
        qualityPreviousButton.onClick.AddListener(OnQualityPreviousClicked);
        submitNicknameButton.onClick.AddListener(OnSubmitNicknameClicked);

        // Sliders
        cameraSensitivitySlider.onValueChanged.AddListener(OnCameraSensitivityChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);


        LoadSettings();
        CheckForNickname();
    }
    private void CheckForNickname()
    {
        if (PlayerPrefs.HasKey("Nickname"))
        {
            string nickname = PlayerPrefs.GetString("Nickname");
            nickNameText.text = nickname;
            inputPanel.transform.parent.gameObject.SetActive(false);
            inputPanel.SetActive(false);
        }
        else
        {
            inputPanel.transform.parent.gameObject.SetActive(true);
            inputPanel.SetActive(true);
        }
    }

    private void OnSubmitNicknameClicked()
    {
        string nickname = nicknameInputField.text;

        if (!string.IsNullOrEmpty(nickname))
        {
            PlayerPrefs.SetString("Nickname", nickname);
            PlayerPrefs.Save();

            nickNameText.text = nickname;
            inputPanel.transform.parent.gameObject.SetActive(false);
            inputPanel.SetActive(false);
            nickNameErrorText.gameObject.SetActive(false);
        }
        else
        {
            nickNameErrorText.gameObject.SetActive(true);
            Debug.Log("Nickname cannot be empty!");
        }
    }
    private void OnNewGameClicked()
    {
        popupManager.ShowPopup("Are you sure you want to start a new game?", "NEW GAME", newGameSprite, ConfirmNewGame);
    }

    private void OnQuitGameClicked()
    {
        popupManager.ShowPopup("Are you sure you want to quit the game?", "EXIT", quitSprite, ConfirmQuitGame);
    }

    private void ConfirmNewGame()
    {
        Debug.Log("New Game Started!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    private void ConfirmQuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }

    private void OnCameraSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("CameraSensitivity", value);
        cameraSensitivityText.text = (value).ToString("F2");
        Debug.Log("Camera Sensitivity Changed: " + value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        musicVolumeText.text = (value * 100).ToString("0");
        Debug.Log("Music Volume Changed: " + value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        sfxVolumeText.text = (value * 100).ToString("0");
        Debug.Log("SFX Volume Changed: " + value);
    }

    private void OnUIVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("UIVolume", value);
        uiVolumeText.text = (value * 100).ToString("0");
        Debug.Log("UI Volume Changed: " + value);
    }

    private void OnQualityNextClicked()
    {
        currentQualityLevel = (currentQualityLevel + 1) % QualitySettings.names.Length;
        UpdateQualitySettings();
    }

    private void OnQualityPreviousClicked()
    {
        currentQualityLevel--;
        if (currentQualityLevel < 0)
        {
            currentQualityLevel = QualitySettings.names.Length - 1;
        }
        UpdateQualitySettings();
    }

    private void UpdateQualitySettings()
    {
        QualitySettings.SetQualityLevel(currentQualityLevel);
        qualityText.text = QualitySettings.names[currentQualityLevel];
        PlayerPrefs.SetInt("Quality", currentQualityLevel);
        Debug.Log("Quality Changed: " + QualitySettings.names[currentQualityLevel]);
    }
    private void OpenLeaderboardPanel()
    {
        leaderboardPanel.transform.parent.gameObject.SetActive(true);
        leaderboardPanel.SetActive(true);
    }
    private void CloseLeaderboardPanel()
    {
        leaderboardPanel.SetActive(false);
        leaderboardPanel.transform.parent.gameObject.SetActive(false);
    }
    private void OpenCreditsPanel()
    {
        creditsPanel.transform.parent.gameObject.SetActive(true);
        creditsPanel.SetActive(true);
    }
    private void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
        creditsPanel.transform.parent.gameObject.SetActive(false);
    }
    private void OpenControllerPanel()
    {
        controllerPanel.transform.parent.gameObject.SetActive(true);
        controllerPanel.SetActive(true);
    }
    private void CloseControllerPanel()
    {
        controllerPanel.SetActive(false);
        controllerPanel.transform.parent.gameObject.SetActive(false);
    }
    private void LoadSettings()
    {
        float defaultSensitivity = 0.5f;
        float defaultMusicVolume = 0.5f;
        float defaultSFXVolume = 0.5f;
        float defaultUIVolume = 0.5f;
        int defaultQualityLevel = 2;

        if (PlayerPrefs.HasKey("CameraSensitivity"))
        {
            float sensitivity = PlayerPrefs.GetFloat("CameraSensitivity");
            cameraSensitivitySlider.value = sensitivity;
        }
        else
        {
            cameraSensitivitySlider.value = defaultSensitivity;
            cameraSensitivityText.text = (defaultSensitivity).ToString("F2");
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicVolumeSlider.value = musicVolume;
        }
        else
        {
            musicVolumeSlider.value = defaultMusicVolume;
            musicVolumeText.text = ((int)(defaultMusicVolume * 100)).ToString("0");
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            sfxVolumeSlider.value = sfxVolume;
        }
        else
        {
            sfxVolumeSlider.value = defaultSFXVolume;
            sfxVolumeText.text = ((int)(defaultSFXVolume * 100)).ToString("0");
        }

        if (PlayerPrefs.HasKey("UIVolume"))
        {
            float uiVolume = PlayerPrefs.GetFloat("UIVolume");
            uiVolumeSlider.value = uiVolume;
        }
        else
        {
            uiVolumeSlider.value = defaultUIVolume;
            uiVolumeText.text = ((int)(defaultUIVolume * 100)).ToString("0");
        }

        if (PlayerPrefs.HasKey("Quality"))
        {
            currentQualityLevel = PlayerPrefs.GetInt("Quality");
        }
        else
        {
            currentQualityLevel = defaultQualityLevel;
        }
        UpdateQualitySettings();
    }
}

