using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedColorChangeButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Referance")]
    [SerializeField] private Button button;
    [SerializeField] private Image targetImage;
    [SerializeField] private Image targetBorderImage;
    [SerializeField] private Image targetIconImage;
    [SerializeField] private TextMeshProUGUI targetText;

    [Header("Color")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.yellow;
    [SerializeField] private Color pressedColor = Color.red;
    [SerializeField] private Color normalTextColor = Color.white;
    [SerializeField] private Color highlightedTextColor = Color.black;
    [SerializeField] private Color pressedTextColor = Color.black;

    [Header("Size")]
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 highlightedScale = Vector3.one * 1.1f;
    [SerializeField] private float animationDuration = 0.15f;

    [Header("Settings")]
    [SerializeField] private bool keepPressedColor = false;
    [SerializeField] private bool defaultSelected = false;
    [SerializeField] private bool inverseControls = false;
    private bool isSelected = false;

    private RectTransform targetRectTransfrom;

    private void Start()
    {
        if (inverseControls) targetRectTransfrom = button.GetComponent<RectTransform>();
        if (defaultSelected)
        {
            Select();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (keepPressedColor)
        {
            Select();
        }
        targetImage.color = normalColor;
        targetIconImage.color = pressedColor;
        targetText.color = pressedColor;
        button.transform.DOScale(normalScale, animationDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            SetHighlightState(true);
        }

        targetImage.color = highlightedColor;
        button.transform.DOScale(highlightedScale, animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            SetHighlightState(false);
        }

        targetImage.color = normalColor;
        button.transform.DOScale(normalScale, animationDuration);
    }

    private void Select()
    {
        isSelected = true;
        SetPressedState();
    }

    public void Deselect()
    {
        isSelected = false;
        SetNormalState();
    }

    private void SetHighlightState(bool highlight)
    {
        var color = highlight ? highlightedColor : normalColor;
        var textColor = highlight ? highlightedTextColor : normalTextColor;

        targetImage.color = color;

        if (inverseControls)
        {
            targetRectTransfrom.DOAnchorPosY(-16, 0.15f);
            if (targetBorderImage != null) targetBorderImage.color = pressedTextColor;
        }
        else
        {
            if (targetBorderImage != null) targetBorderImage.color = normalTextColor;
        }

        if (targetIconImage != null) targetIconImage.color = textColor;
        if (targetText != null) targetText.color = textColor;
    }

    private void SetPressedState()
    {
        targetImage.color = pressedColor;
        if (inverseControls)
        {
            targetRectTransfrom.DOAnchorPosY(-32, 0.15f);
            if (targetBorderImage != null) targetBorderImage.color = pressedTextColor;
        }
        else
        {
            if (targetBorderImage != null) targetBorderImage.color = pressedColor;
        }

        if (targetIconImage != null) targetIconImage.color = pressedTextColor;
        if (targetText != null) targetText.color = pressedTextColor;
    }

    private void SetNormalState()
    {
        targetImage.color = normalColor;

        if (inverseControls)
        {
            targetRectTransfrom.DOAnchorPosY(-16, 0.15f);
            if (targetBorderImage != null) targetBorderImage.color = normalTextColor;
        }
        else
        {
            if (targetBorderImage != null) targetBorderImage.color = normalTextColor;
        }

        if (targetIconImage != null) targetIconImage.color = normalTextColor;
        if (targetText != null) targetText.color = normalTextColor;
    }
}
