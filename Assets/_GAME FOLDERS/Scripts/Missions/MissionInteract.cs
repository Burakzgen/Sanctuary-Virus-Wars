using System;
using UnityEngine;
using UnityEngine.Events;

public class MissionInteract : MonoBehaviour, IInteractable
{
    public UnityEvent OnMissionCompleted;
    public event Action OnCollected;
    public bool useCharacterControls = false;
    public void Interact(PlayerHealth health = null)
    {
        OnMissionCompleted?.Invoke();
        if (useCharacterControls)
            GameManager.Instance.PauseChracterControls();
    }
}
