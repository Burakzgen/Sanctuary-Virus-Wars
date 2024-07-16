using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChangeButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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

    [Header("Settings")]
    [SerializeField] private bool keepPressedColor = false;
    [SerializeField] private List<ColorChangeButton> groupButtons;
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
        if (!keepPressedColor) return;

        foreach (var btn in groupButtons)
        {
            if (btn != this)
            {
                btn.Deselect();
            }
        }

        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected) return;

        SetHighlightState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;

        SetHighlightState(false);
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

        if (targetBorderImage != null) targetBorderImage.color = normalTextColor;
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
