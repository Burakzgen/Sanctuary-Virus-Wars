using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isLocked = false;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openDuration = 1f;

    private bool isOpen = false;
    private Tween doorTween;

    public event System.Action OnCollected;

    public void Interact(PlayerHealth health = null)
    {
        if (isLocked)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSounds[1]);
        }
        else if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    private void OpenDoor()
    {
        isOpen = true;
        AnimateDoor(openAngle);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSounds[2]);
    }

    private void CloseDoor()
    {
        isOpen = false;
        AnimateDoor(0f);
    }

    private void AnimateDoor(float targetAngle)
    {
        if (doorTween != null && doorTween.IsActive())
        {
            doorTween.Kill();
        }

        doorTween = transform.DORotate(new Vector3(0, targetAngle, 0), openDuration)
            .SetEase(Ease.InOutQuad);

    }
    private void OnDestroy()
    {
        doorTween?.Kill();
    }
}