using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Image popupIconImage;
    [SerializeField] private TextMeshProUGUI popupTitleText;
    [SerializeField] private TextMeshProUGUI popupMessage;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private System.Action confirmCallback;

    private void Start()
    {
        popupPanel.SetActive(false);
        popupPanel.transform.parent.gameObject.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
    }

    public void ShowPopup(string message, string title, Sprite iconSprite, System.Action confirmAction)
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
        popupMessage.text = message;
        confirmCallback = confirmAction;
        popupIconImage.sprite = iconSprite;
        popupTitleText.text = title;
        popupPanel.SetActive(true);
        popupPanel.transform.parent.gameObject.SetActive(true);
    }

    private void OnConfirm()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
        confirmCallback?.Invoke();
        popupPanel.SetActive(false);
        popupPanel.transform.parent.gameObject.SetActive(false);
    }

    private void OnCancel()
    {
        AudioManager.Instance.PlayUI(AudioManager.Instance.buttonClickSound);
        popupPanel.SetActive(false);
        popupPanel.transform.parent.gameObject.SetActive(false);
    }
}
