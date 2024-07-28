using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class TipPanelController : MonoBehaviour
{
    [Header("Mission Panel")]
    [SerializeField] private RectTransform tipPanel;
    [SerializeField] private TextMeshProUGUI tipText;

    [Header("Mission Canvas Panel")]
    [SerializeField] private CanvasGroup canvasMainPanel;
    [SerializeField] private CanvasGroup canvasChildPanel;
    [SerializeField] private CanvasGroup tipInfoPanel;


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

    [SerializeField] KeyCode keyCode;
    private void Start()
    {
        tipPanel.anchoredPosition = offScreenPosition;
        tipPanel.sizeDelta = offSize;
        tipInfoPanel.alpha = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode) && Time.time > lastToggleTime + cooldownTime)
        {
            if (hasMission)
            {
                isAutoOpened = false;
                ToggleTipPanel();
                lastToggleTime = Time.time;
            }
        }
    }
    public void ShowTipPanel()
    {
        hasMission = true;
        tipInfoPanel.DOFade(1, animationDuration);
        isAutoOpened = true;
        if (!isPanelVisible)
        {
            ToggleTipPanel();
            StartCoroutine(CloseTipPanelAfterDelay(3f));
        }
    }
    private IEnumerator CloseTipPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isPanelVisible && isAutoOpened)
        {
            ToggleTipPanel();
        }
    }
    private void ToggleTipPanel()
    {
        if (isPanelVisible)
        {
            canvasChildPanel.DOFade(0, animationDuration).OnComplete(() =>
            {
                tipPanel.DOAnchorPos(offScreenPosition, animationDuration);
                tipPanel.DOSizeDelta(offSize, animationDuration);
                canvasMainPanel.DOFade(0, animationDuration);
                tipInfoPanel.DOFade(1, animationDuration);
            });
        }
        else
        {
            tipInfoPanel.DOFade(0, animationDuration);
            tipPanel.DOAnchorPos(onScreenPosition, animationDuration);
            tipPanel.DOSizeDelta(onSize, animationDuration);
            canvasMainPanel.DOFade(1, animationDuration).OnComplete(() =>
            {
                canvasChildPanel.DOFade(1, animationDuration);

            });
        }
        isPanelVisible = !isPanelVisible;
    }
    public void SetMissionText(string mission)
    {
        tipText.text = mission;
    }
    public void HideTabInfo()
    {
        tipInfoPanel.DOFade(0, animationDuration);
        hasMission = false;
    }
}
