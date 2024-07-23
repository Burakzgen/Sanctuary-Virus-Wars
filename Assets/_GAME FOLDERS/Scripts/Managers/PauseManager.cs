using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button countinueButton;
    [SerializeField] private Button quitGameButton;
    [SerializeField] private Button quitGameCancelButton;
    [SerializeField] private Button quitGameConfirmButton;
    [SerializeField] private Button settingOpenButton;
    [SerializeField] private Button settingCloseButton;
    [SerializeField] private Button qualityNextButton;
    [SerializeField] private Button qualityPreviousButton;


    [Header("Panel")]
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject quitGamePopupPanel;
    [SerializeField] private GameObject mainPausePanel;
    [SerializeField] private GameObject childPausePanel;

    [Header("Sliders")]
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI cameraSensitivityText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI uiVolumeText;

    private int currentQualityLevel;

    private void Start()
    {
        // Buttons
        quitGameButton.onClick.AddListener(() => OnQuitGameClicked(true));
        quitGameCancelButton.onClick.AddListener(() => OnQuitGameClicked(false));
        countinueButton.onClick.AddListener(OnContinueButtonClicked);
        quitGameConfirmButton.onClick.AddListener(ConfirmQuitGame);
        settingOpenButton.onClick.AddListener(ShowSettingPanel);
        settingCloseButton.onClick.AddListener(HideSettingPanel);
        qualityNextButton.onClick.AddListener(OnQualityNextClicked);
        qualityPreviousButton.onClick.AddListener(OnQualityPreviousClicked);

        // Sliders
        cameraSensitivitySlider.onValueChanged.AddListener(OnCameraSensitivityChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeChanged);


        LoadSettings();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.IsGamePaused() && !childPausePanel.activeSelf)
            {
                OnContinueButtonClicked();
            }
            else
            {
                PauseGame();
            }
        }
    }
    private void PauseGame()
    {
        GameManager.Instance.PauseGame();
        mainPausePanel.SetActive(true);
    }

    private void OnContinueButtonClicked()
    {
        GameManager.Instance.ContinueGame();
        mainPausePanel.SetActive(false);
    }
    private void OnQuitGameClicked(bool value)
    {
        childPausePanel.gameObject.SetActive(!value);
        quitGamePopupPanel.transform.parent.gameObject.SetActive(value);
        quitGamePopupPanel.gameObject.SetActive(value);
    }
    private void ConfirmQuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
    }
    private void ShowSettingPanel()
    {
        settingPanel.SetActive(true);
    }
    private void HideSettingPanel()
    {
        settingPanel.SetActive(false);
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
