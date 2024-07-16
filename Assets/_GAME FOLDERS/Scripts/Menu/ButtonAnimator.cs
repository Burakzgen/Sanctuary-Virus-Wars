using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
    [Header("Referance")]
    [SerializeField] private Button button;
    [SerializeField] private Image targetImage;
    [SerializeField] private Image targetIconImage;
    [SerializeField] private Image targetBorderImage;
    [SerializeField] private TextMeshProUGUI targetText;

    [Header("Sprite")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    [SerializeField] private Sprite pressedSprite;

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
    [SerializeField] private Vector3 pressedScale = Vector3.one * 0.9f;
    [SerializeField] private float animationDuration = 0.2f;

    [Header("Settings")]
    [SerializeField] private bool useColorChange = false;
    [SerializeField] private bool useSpriteChange = false;
    [SerializeField] private bool useTextColorChange = false;
    [SerializeField] private bool useScaleAnimation = false;
    [SerializeField] private bool keepPressedColor = false;
    [SerializeField] private bool resetOtherButtons = false;

    private static ButtonAnimator currentlySelectedButton = null;

    private void Start()
    {
        if (targetImage == null)
            targetImage = button.GetComponent<Image>();

        // Event Trigger component ekleyerek hover ve exit olaylarýný dinle
        EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((eventData) => OnPointerEnter());
        eventTrigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((eventData) => OnPointerExit());
        eventTrigger.triggers.Add(pointerExitEntry);

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => OnPointerDown());
        eventTrigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((eventData) => OnPointerUp());
        eventTrigger.triggers.Add(pointerUpEntry);
    }

    private void OnPointerEnter()
    {
        if (keepPressedColor && currentlySelectedButton == this) return;

        if (useSpriteChange)
        {
            targetImage.sprite = highlightedSprite;
        }
        if (useColorChange)
        {
            targetImage.color = highlightedColor;
        }
        if (useTextColorChange && targetText != null)
        {
            targetText.color = highlightedTextColor;
            if (targetIconImage != null)
                targetIconImage.color = highlightedTextColor;
        }
        if (useScaleAnimation)
        {
            button.transform.DOScale(highlightedScale, animationDuration);
        }
        else
        {
            button.transform.localScale = highlightedScale;
        }
    }

    private void OnPointerExit()
    {
        if (keepPressedColor && currentlySelectedButton == this) return;

        if (useSpriteChange)
        {
            targetImage.sprite = normalSprite;
        }
        if (useColorChange)
        {
            targetImage.color = normalColor;
        }
        if (useTextColorChange && targetText != null)
        {
            targetText.color = normalTextColor;
            if (targetIconImage != null)
                targetIconImage.color = normalTextColor;
        }
        if (useScaleAnimation)
        {
            button.transform.DOScale(normalScale, animationDuration);
        }
        else
        {
            button.transform.localScale = normalScale;
        }
    }

    private void OnPointerDown()
    {
        if (resetOtherButtons && currentlySelectedButton != null && currentlySelectedButton != this)
        {
            currentlySelectedButton.ResetButtonState();
        }

        if (useSpriteChange)
        {
            targetImage.sprite = pressedSprite;
        }
        if (useColorChange)
        {
            targetImage.color = pressedColor;
            if (targetBorderImage != null)
                targetBorderImage.color = pressedColor;
        }
        if (useTextColorChange && targetText != null)
        {
            targetText.color = pressedTextColor;
            if (targetIconImage != null)
                targetIconImage.color = pressedTextColor;

        }
        if (useScaleAnimation)
        {
            button.transform.DOScale(pressedScale, animationDuration);
        }
        else
        {
            button.transform.localScale = pressedScale;
        }

        currentlySelectedButton = this;
    }

    private void OnPointerUp()
    {
        if (keepPressedColor) return;

        if (useSpriteChange)
        {
            targetImage.sprite = highlightedSprite;
        }
        if (useColorChange)
        {
            targetImage.color = highlightedColor;
            if (targetBorderImage != null)
                targetBorderImage.color = highlightedColor;
        }
        if (useTextColorChange && targetText != null)
        {
            targetText.color = highlightedTextColor;
            if (targetIconImage != null)
                targetIconImage.color = highlightedTextColor;

        }
        if (useScaleAnimation)
        {
            button.transform.DOScale(highlightedScale, animationDuration);
        }
        else
        {
            button.transform.localScale = highlightedScale;
        }
    }

    public void ResetButtonState()
    {
        if (useSpriteChange)
        {
            targetImage.sprite = normalSprite;
        }
        if (useColorChange)
        {
            targetImage.color = normalColor;
            if (targetBorderImage != null)
                targetBorderImage.color = normalTextColor;
        }
        if (useTextColorChange && targetText != null)
        {
            targetText.color = normalTextColor;
            if (targetIconImage != null)
                targetIconImage.color = normalTextColor;

        }
        if (useScaleAnimation)
        {
            button.transform.DOScale(normalScale, animationDuration);
        }
        else
        {
            button.transform.localScale = normalScale;
        }
    }
}
