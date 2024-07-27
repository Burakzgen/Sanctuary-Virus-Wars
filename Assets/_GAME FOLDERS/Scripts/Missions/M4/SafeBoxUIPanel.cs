using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SafeBoxUIPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI passwordDisplay;
    [SerializeField] private Button[] numberButtons;
    [SerializeField] private Button checkButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button backButton;
    [SerializeField] private string correctPassword = "1234";
    [SerializeField] private TextMeshProUGUI feedbackText;

    private string enteredPassword = "";

    public FirstPersonMovement m_FirstPersonMovement;
    public FirstPersonLook m_FirstPersonLook;
    public FirstPersonZoom m_FirstPersonZoom;
    [SerializeField] MissionInteraction m_MissionInteraction;

    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float duration = 3f;

    private void Start()
    {
        foreach (Button button in numberButtons)
        {
            button.onClick.AddListener(() => OnNumberButtonClicked(button.GetComponentInChildren<TextMeshProUGUI>().text));
        }

        checkButton.onClick.AddListener(OnCheckButtonClicked);
        clearButton.onClick.AddListener(OnClearButtonClicked);
        clearButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnNumberButtonClicked(string number)
    {
        if (enteredPassword.Length < 4)
        {
            enteredPassword += number;
            passwordDisplay.text = enteredPassword;
            feedbackText.text = "";
        }
    }

    private void OnCheckButtonClicked()
    {
        if (enteredPassword == correctPassword)
        {
            feedbackText.text = "Correct password!";
            m_MissionInteraction.OnMissionCompleted();
            objectToRotate.DORotate(new Vector3(0, -30, 0), duration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);

            gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            m_FirstPersonZoom.enabled = true;
            m_FirstPersonMovement.enabled = true;
            m_FirstPersonLook.enabled = true;
        }
        else
        {
            feedbackText.text = "Incorrect password!";
        }

        enteredPassword = "";
        passwordDisplay.text = enteredPassword;
    }
    private void OnClearButtonClicked()
    {
        enteredPassword = "";
        passwordDisplay.text = enteredPassword;
        feedbackText.text = "";
    }
    private void OnBackButtonClicked()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        m_FirstPersonZoom.enabled = true;
        m_FirstPersonMovement.enabled = true;
        m_FirstPersonLook.enabled = true;
    }
}
