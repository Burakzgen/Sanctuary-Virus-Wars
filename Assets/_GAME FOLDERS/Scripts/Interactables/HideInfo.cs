using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HideInfo : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(HidePanel());
    }
    IEnumerator HidePanel()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        gameObject.GetComponent<RectTransform>().DOAnchorPosX(-1000f, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
