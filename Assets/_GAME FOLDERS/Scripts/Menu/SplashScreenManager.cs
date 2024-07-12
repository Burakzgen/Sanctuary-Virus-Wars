using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    [SerializeField] GameObject splashPanel;
    [SerializeField] RectTransform splashContent;

    void Start()
    {
        AnimateText();
        StartCoroutine(WaitForAnyKey());
    }

    void AnimateText()
    {
        splashContent.DOAnchorPosY(4, 1.2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    IEnumerator WaitForAnyKey()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        splashContent.GetComponent<CanvasGroup>().DOFade(0, 1f);
        splashContent.DOAnchorPosY(splashContent.anchoredPosition.y + 50, 1f).OnComplete(() =>
        {
            splashPanel.SetActive(false);
        });
    }
}
