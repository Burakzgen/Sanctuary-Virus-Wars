using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject loadingIamgeIcon;
    [SerializeField] private RectTransform loadingContent;
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private Image loadingBackground;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private float initialLoadingDuration = 3f;

    private bool isInitialLoading = true;

    private void Start()
    {
        ShowMenuLoadingScreen();
    }

    private void AnimateSplashText()
    {
        loadingContent.DOAnchorPosY(4, 1.2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    private void ShowMenuLoadingScreen()
    {
        loadingPanel.SetActive(true);
        AnimateSplashText();
        StartCoroutine(InitialLoading());
    }

    public void ShowGameLoadingScreen()
    {
        loadingPanel.SetActive(true);
        AnimateSplashText();
        StartCoroutine(LoadGameScene("Game"));
    }
    public void ShowGameLoadingScreenFromGame()
    {
        loadingPanel.SetActive(true);
        AnimateSplashText();
        StartCoroutine(LoadGameScene("Game"));
    }
    public void ShowMenuLoadingScreenFromGame()
    {
        loadingPanel.SetActive(true);
        AnimateSplashText();
        StartCoroutine(LoadGameScene("Menu"));
    }

    private IEnumerator InitialLoading()
    {
        loadingText.text = "LOADING";
        loadingIamgeIcon.SetActive(true);
        loadingSlider.GetComponent<CanvasGroup>().DOFade(1, 0.15f);

        yield return new WaitForSeconds(initialLoadingDuration);

        loadingText.text = "ANY KEY";
        loadingIamgeIcon.SetActive(false);
        loadingSlider.GetComponent<CanvasGroup>().DOFade(0, 0.15f);

        yield return WaitForAnyKeyToContinue();
    }

    private IEnumerator LoadGameScene(string scene)
    {
        if (!isInitialLoading)
        {
            loadingSlider.GetComponent<CanvasGroup>().DOFade(1, 0.15f);
            loadingIamgeIcon.SetActive(true);
            loadingText.text = "LOADING";
        }

        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            if (isInitialLoading)
            {
                loadingText.text = "LOADING";
            }

            yield return null;
        }

        loadingText.text = "ANY KEY";
        loadingIamgeIcon.SetActive(false);
        loadingSlider.GetComponent<CanvasGroup>().DOFade(0, 0.15f);
        yield return WaitForAnyKeyToContinue();
    }

    private IEnumerator WaitForAnyKeyToContinue()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }


        yield return new WaitForSeconds(0.1f);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSound);

        loadingCanvasGroup.DOFade(0, 1f).OnComplete(() =>
        {
            loadingPanel.SetActive(false);
            loadingCanvasGroup.alpha = 1;
            loadingContent.DOAnchorPosY(0, 0);
        });

        isInitialLoading = false;
    }
}
