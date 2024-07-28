using UnityEngine;
using UnityEngine.UI;

public class CheckPrivateFolder : MonoBehaviour
{
    [SerializeField] private Image mainImage;
    [SerializeField] private Sprite[] images;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button preButton;

    private int currentIndex = 0;

    private void Start()
    {
        if (images.Length > 0)
        {
            mainImage.sprite = images[0];
        }

        nextButton.onClick.AddListener(ShowNextImage);
        preButton.onClick.AddListener(ShowPreviousImage);

        preButton.gameObject.SetActive(false);
    }

    private void ShowNextImage()
    {
        if (currentIndex < images.Length - 1)
        {
            currentIndex++;
            mainImage.sprite = images[currentIndex];

            preButton.gameObject.SetActive(true);

            if (currentIndex == images.Length - 1)
            {
                nextButton.gameObject.SetActive(false);
            }
        }
    }

    private void ShowPreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            mainImage.sprite = images[currentIndex];

            nextButton.gameObject.SetActive(true);

            if (currentIndex == 0)
            {
                preButton.gameObject.SetActive(false);
            }
        }
    }
}
