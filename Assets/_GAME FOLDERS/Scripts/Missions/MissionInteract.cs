using System;
using UnityEngine;
using UnityEngine.Events;

public class MissionInteract : MonoBehaviour, IInteractable
{
    public UnityEvent OnMissionCompleted;
    public event Action OnCollected;
    public FirstPersonMovement m_FirstPersonMovement;
    public FirstPersonLook m_FirstPersonLook;
    public FirstPersonZoom m_FirstPersonZoom;
    public void Interact(PlayerHealth health = null)
    {
        OnMissionCompleted?.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
        m_FirstPersonZoom.enabled = false;
        m_FirstPersonMovement.enabled = false;
        m_FirstPersonLook.enabled = false;
    }
}
