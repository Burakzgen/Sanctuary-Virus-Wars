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
    [SerializeField] private Slider masterVolumeSlider;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI cameraSensitivityText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private TextMeshProUGUI uiVolumeText;
    [SerializeField] private TextMeshProUGUI masterVolumeText;

    private int currentQualityLevel;
    [SerializeField] FirstPersonLook m_FirstPersonLook;

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
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);


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
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void OnQuitGameClicked(bool value)
    {
        childPausePanel.gameObject.SetActive(!value);
        quitGamePopupPanel.transform.parent.gameObject.SetActive(value);
        quitGamePopupPanel.gameObject.SetActive(value);
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void ConfirmQuitGame()
    {
        Debug.Log("Game Quit!");
        Application.Quit();
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void ShowSettingPanel()
    {
        settingPanel.SetActive(true);
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void HideSettingPanel()
    {
        settingPanel.SetActive(false);
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void OnCameraSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("CameraSensitivity", value);
        cameraSensitivityText.text = (value).ToString("F2");
        m_FirstPersonLook.SetSensitivty();
        Debug.Log("Camera Sensitivity Changed: " + value);
    }
    private void OnMasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        masterVolumeText.text = (value * 100).ToString("0");
        AudioManager.Instance.SetMasterVolume(value);
        Debug.Log("UI Volume Changed: " + value);
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
    private void OnQualityNextClicked()
    {
        currentQualityLevel = (currentQualityLevel + 1) % QualitySettings.names.Length;
        UpdateQualitySettings();
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
    }
    private void OnQualityPreviousClicked()
    {
        currentQualityLevel--;
        if (currentQualityLevel < 0)
        {
            currentQualityLevel = QualitySettings.names.Length - 1;
        }
        UpdateQualitySettings();
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
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
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float uiVolume = PlayerPrefs.GetFloat("MasterVolume");
            masterVolumeSlider.value = uiVolume;
            AudioManager.Instance.SetMasterVolume(uiVolume);
        }
        else
        {
            masterVolumeSlider.value = defaultUIVolume;
            masterVolumeText.text = ((int)(defaultUIVolume * 100)).ToString("0");
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
