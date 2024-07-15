using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 highlightedScale = Vector3.one * 1.1f;
    [SerializeField] private Vector3 pressedScale = Vector3.one * 0.9f;
    [SerializeField] private float animationDuration = 0.2f;

    private Image buttonImage;

    private void Start()
    {
        buttonImage = button.GetComponent<Image>();
        button.onClick.AddListener(OnButtonClicked);

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
        buttonImage.sprite = highlightedSprite;
        button.transform.DOScale(highlightedScale, animationDuration);
    }

    private void OnPointerExit()
    {
        buttonImage.sprite = normalSprite;
        button.transform.DOScale(normalScale, animationDuration);
    }

    private void OnPointerDown()
    {
        buttonImage.sprite = pressedSprite;
        button.transform.DOScale(pressedScale, animationDuration);
    }

    private void OnPointerUp()
    {
        buttonImage.sprite = highlightedSprite;
        button.transform.DOScale(highlightedScale, animationDuration);
    }

    private void OnButtonClicked()
    {
        // Ek animasyonlar veya iþlemler yapýlabilir
        Debug.Log("On Button Clicked");
    }
}
