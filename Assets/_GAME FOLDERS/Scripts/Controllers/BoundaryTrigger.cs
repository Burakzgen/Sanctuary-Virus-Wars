using DG.Tweening;
using UnityEngine;

public class BoundaryTrigger : MonoBehaviour
{
    [SerializeField] CanvasGroup boundaryWarningImage;
    IHealth health;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            health = other.GetComponent<PlayerHealth>();
            boundaryWarningImage.DOFade(1, 0.25f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            health.TakeDamage(0.5f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boundaryWarningImage.DOFade(0, 0.25f);
        }
    }
}
