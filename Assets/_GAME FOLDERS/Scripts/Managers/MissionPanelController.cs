using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class MissionPanelController : MonoBehaviour
{
    [Header("Mission Panel")]
    [SerializeField] private RectTransform missionPanel;
    [SerializeField] private TextMeshProUGUI missionText;

    [Header("Canvas Panel")]
    [SerializeField] private CanvasGroup canvasMainPanel;
    [SerializeField] private CanvasGroup canvasChildPanel;
    [SerializeField] private CanvasGroup tabInfoPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;

    [Header("Default Position and Size")]
    [SerializeField] private Vector2 offScreenPosition;
    [SerializeField] private Vector2 onScreenPosition;
    [SerializeField] private Vector2 offSize;
    [SerializeField] private Vector2 onSize;

    private bool isPanelVisible = false;
    private bool isAutoOpened = false;


    private void Start()
    {
        missionPanel.anchoredPosition = offScreenPosition;
        missionPanel.sizeDelta = offSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isAutoOpened = false;
            ToggleMissionPanel();
        }
    }
    public void ShowMissionPanel()
    {
        isAutoOpened = true;
        if (!isPanelVisible)
        {
            ToggleMissionPanel();
            StartCoroutine(CloseMissionPanelAfterDelay(3f));
        }
    }
    private IEnumerator CloseMissionPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
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
        gameOverPanel.SetActive(true);
    }
    public void SetMissionText(string mission)
    {
        missionText.text = mission;
    }
}
