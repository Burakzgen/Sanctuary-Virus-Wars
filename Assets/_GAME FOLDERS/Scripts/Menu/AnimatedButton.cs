using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Referance")]
    [SerializeField] private Button button;
    [SerializeField] private Image targetImage;

    [Header("Color")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightedColor = Color.yellow;

    [Header("Size")]
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 highlightedScale = Vector3.one * 1.1f;
    [SerializeField] private float animationDuration = 0.2f;
    [Header("Settings")]
    [SerializeField] bool offSound = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.color = highlightedColor;
        button.transform.DOScale(highlightedScale, animationDuration);
        if (!offSound)
            AudioManager.Instance.PlayUI(AudioManager.Instance.hoverClickSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.color = normalColor;
        button.transform.DOScale(normalScale, animationDuration);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        targetImage.color = normalColor;
        button.transform.DOScale(normalScale, animationDuration);
    }
}
