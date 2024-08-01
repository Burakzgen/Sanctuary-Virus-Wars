using UnityEngine;
using UnityEngine.UI;

public class CheckMail : MonoBehaviour
{

    [SerializeField] private Button closeButton;
    [SerializeField] MissionCompletionInteraction m_MissionInteraction;
    private void Start()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }
    void OnCloseButtonClick()
    {
        gameObject.SetActive(false);
    }
    public void OnCompletedButtonClick()
    {
        GameManager.Instance.ResumeChracterControls(true);
        m_MissionInteraction.OnMissionCompleted();
    }
}
