using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanelController : MonoBehaviour
{
    [Header("Mission Panel")]
    [SerializeField] private RectTransform missionPanel;
    [SerializeField] private TextMeshProUGUI missionText;

    [Header("Mission Canvas Panel")]
    [SerializeField] private CanvasGroup canvasMainPanel;
    [SerializeField] private CanvasGroup canvasChildPanel;
    [SerializeField] private CanvasGroup tabInfoPanel;
    [SerializeField] private RectTransform missionCompletePopup;


    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;

    [Header("Default Position and Size")]
    [SerializeField] private Vector2 offScreenPosition;
    [SerializeField] private Vector2 onScreenPosition;
    [SerializeField] private Vector2 offSize;
    [SerializeField] private Vector2 onSize;

    private bool isPanelVisible = false;
    private bool isAutoOpened = false;
    private bool hasMission = false;

    private float cooldownTime = 0.5f; // sürekli týklamayý önlemek için eklendi
    private float lastToggleTime;

    [SerializeField] private GameObject completedGame;
    [SerializeField] private GameObject gamePanelsObject;
    [SerializeField] private GameObject radarObject;
    [SerializeField] private GameObject quitButtonObject;
    [SerializeField] private GameObject creditObject;
    [SerializeField] private GameObject curserObject;
    [SerializeField] private Image fadeImage;
    private void Start()
    {
        missionPanel.anchoredPosition = offScreenPosition;
        missionPanel.sizeDelta = offSize;
        tabInfoPanel.alpha = 0f;
        fadeImage.DOFade(0, 3f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Time.time > lastToggleTime + cooldownTime)
        {
            if (hasMission)
            {
                isAutoOpened = false;
                ToggleMissionPanel();
                lastToggleTime = Time.time;
            }
        }
    }
    public void ShowMissionPanel()
    {
        hasMission = true;
        tabInfoPanel.DOFade(1, animationDuration);
        isAutoOpened = true;
        if (!isPanelVisible)
        {
            ToggleMissionPanel();
            StartCoroutine(CloseMissionPanelAfterDelay(4f));
        }
    }
    private IEnumerator CloseMissionPanelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (isPanelVisible && isAutoOpened)
        {
            ToggleMissionPanel();
        }
    }
    private void ToggleMissionPanel()
    {
        if (isPanelVisible)
        {
            canvasChildPanel.DOFade(0, animationDuration).OnComplete(() =>
            {
                missionPanel.DOAnchorPos(offScreenPosition, animationDuration);
                missionPanel.DOSizeDelta(offSize, animationDuration);
                canvasMainPanel.DOFade(0, animationDuration);
                tabInfoPanel.DOFade(1, animationDuration);
            });
        }
        else
        {
            tabInfoPanel.DOFade(0, animationDuration);
            missionPanel.DOAnchorPos(onScreenPosition, animationDuration);
            missionPanel.DOSizeDelta(onSize, animationDuration);
            canvasMainPanel.DOFade(1, animationDuration).OnComplete(() =>
            {
                canvasChildPanel.DOFade(1, animationDuration);

            });
        }
        isPanelVisible = !isPanelVisible;
    }
    public void ShowGameOver()
    {
        GameManager.Instance.GameOver();
    }
    private IEnumerator CompletedGameDelay()
    {
        radarObject.SetActive(false);
        gamePanelsObject.SetActive(false);
        GameManager.Instance.PauseChracterControls();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(ShowGameOverSequence());
    }
    public void ShowCompletedGame()
    {
        StartCoroutine(CompletedGameDelay());
    }
    private IEnumerator ShowGameOverSequence()
    {
        yield return fadeImage.DOFade(1, 1.5f).WaitForCompletion();

        completedGame.SetActive(true);
        quitButtonObject.SetActive(true);
        creditObject.SetActive(true);

        yield return fadeImage.DOFade(0, 1.5f).WaitForCompletion();
    }
    public void SetMissionText(string mission)
    {
        missionText.text = mission;
    }
    public void ShowMissionCompletePopup()
    {
        missionCompletePopup.DOAnchorPos(new Vector2(0f, -96f), 0.4f).OnComplete(() =>
        {
            missionCompletePopup.DOSizeDelta(new Vector2(448f, 64f), 0.4f).OnComplete(() =>
            {
                missionCompletePopup.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(1, 0.3f);
            });
        });

        StartCoroutine(HidePopupAfterDelay());
    }
    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        missionCompletePopup.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 0.3f).OnComplete(() =>
        {
            missionCompletePopup.DOSizeDelta(new Vector2(16f, 64f), 0.25f).OnComplete(() =>
            {
                missionCompletePopup.DOAnchorPos(new Vector2(0f, 64f), 0.25f);
            });
        });
    }
    public void HideTabInfo()
    {
        tabInfoPanel.DOFade(0, animationDuration).SetUpdate(true);
        hasMission = false;
    }
}
