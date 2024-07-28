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
        GameManager.Instance.ResumeChracterControls();
        m_MissionInteraction.OnMissionCompleted();
        gameObject.SetActive(false);
    }
}
