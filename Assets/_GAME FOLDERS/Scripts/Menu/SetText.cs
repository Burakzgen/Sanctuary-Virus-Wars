using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetText : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI titleTextComp;
    [SerializeField] TextMeshProUGUI descTextComp;
    [SerializeField] string titleText;
    [SerializeField] string descText;

    [Header("Size")]
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 highlightedScale = Vector3.one * 1.1f;
    [SerializeField] private float animationDuration = 0.15f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        iconImage.transform.DOScale(highlightedScale, animationDuration).OnComplete(() =>
        {
            iconImage.transform.DOScale(normalScale, 0.05f);
        });
        titleTextComp.text = titleText;
        descTextComp.text = descText;
    }
}
