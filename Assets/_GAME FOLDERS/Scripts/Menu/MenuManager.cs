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
    [SerializeField] private Button changeNicknameButton;

    [Header("Sprites")]
    [SerializeField] private Sprite newGameSprite;
    [SerializeField] private Sprite quitSprite;

    [Header("Sliders")]
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;
    [SerializeField] private Slider masterVolumeSlider;

    [Header("Input Fields")]
    public TMP_InputField nicknameInputField;

    [Header("Text")]
    public TextMeshProUGUI nickNameText;
    [SerializeField] private TextMeshProUGUI nickNameErrorText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI cameraSensitivityText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI uiVolumeText;
    [SerializeField] private TextMeshProUGUI masterVolumeText;

    [Header("Panels")]
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject controllerPanel;
    public GameObject inputPanel;

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
        changeNicknameButton.onClick.AddListener(ChangeNickName);

        // Sliders
        cameraSensitivitySlider.onValueChanged.AddListener(OnCameraSensitivityChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);


        LoadSettings();
    }
    private void OnSubmitNicknameClicked()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
        string nickname = nicknameInputField.text;

        if (!string.IsNullOrEmpty(nickname))
        {
            PlayerPrefs.SetString("Nickname", nickname);
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
    private void ChangeNickName()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
        nicknameInputField.text = string.Empty;
        inputPanel.transform.parent.gameObject.SetActive(true);
        inputPanel.SetActive(true);
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
        AudioManager.Instance.SetMusicVolume(value);
        Debug.Log("Music Volume Changed: " + value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        sfxVolumeText.text = (value * 100).ToString("0");
        AudioManager.Instance.SetSFXVolume(value);
        Debug.Log("SFX Volume Changed: " + value);
    }

    private void OnUIVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("UIVolume", value);
        uiVolumeText.text = (value * 100).ToString("0");
        AudioManager.Instance.SetUIVolume(value);
        Debug.Log("UI Volume Changed: " + value);
    }
    private void OnMasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        masterVolumeText.text = (value * 100).ToString("0");
        AudioManager.Instance.SetMasterVolume(value);
        Debug.Log("UI Volume Changed: " + value);
    }
    private void OnQualityNextClicked()
    {
        currentQualityLevel = (currentQualityLevel + 1) % QualitySettings.names.Length;
        UpdateQualitySettings();
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }

    private void OnQualityPreviousClicked()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
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
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void CloseLeaderboardPanel()
    {
        leaderboardPanel.SetActive(false);
        leaderboardPanel.transform.parent.gameObject.SetActive(false);
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void OpenCreditsPanel()
    {
        creditsPanel.transform.parent.gameObject.SetActive(true);
        creditsPanel.SetActive(true);
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
        creditsPanel.transform.parent.gameObject.SetActive(false);
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void OpenControllerPanel()
    {
        controllerPanel.transform.parent.gameObject.SetActive(true);
        controllerPanel.SetActive(true);
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void CloseControllerPanel()
    {
        controllerPanel.SetActive(false);
        controllerPanel.transform.parent.gameObject.SetActive(false);
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void LoadSettings()
    {
        float defaultSensitivity = 2f;
        float defaultMusicVolume = 0.01f;
        float defaultSFXVolume = 0.15f;
        float defaultUIVolume = 0.15f;
        float defaultMasterVolume = 1f;
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
            AudioManager.Instance.SetMusicVolume(musicVolume);
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
            AudioManager.Instance.SetSFXVolume(sfxVolume);
        }
        else
        {
            sfxVolumeSlider.value = defaultSFXVolume;
            sfxVolumeText.text = ((int)(defaultSFXVolume * 100)).ToString("0");
        }
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float uiVolume = PlayerPrefs.GetFloat("MasterVolume");
            masterVolumeSlider.value = uiVolume;
            AudioManager.Instance.SetMasterVolume(uiVolume);
        }
        else
        {
            masterVolumeSlider.value = defaultMasterVolume;
            masterVolumeText.text = ((int)(defaultMasterVolume * 100)).ToString("0");
        }

        if (PlayerPrefs.HasKey("UIVolume"))
        {
            float uiVolume = PlayerPrefs.GetFloat("UIVolume");
            uiVolumeSlider.value = uiVolume;
            AudioManager.Instance.SetUIVolume(uiVolume);
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

