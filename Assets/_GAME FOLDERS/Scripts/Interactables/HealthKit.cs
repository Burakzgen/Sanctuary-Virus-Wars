using UnityEngine;

public class HealthKit : MonoBehaviour, IInteractable
{
    [SerializeField] private float healthAmount = 15f;

    public void Interact()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(healthAmount);
            HealthKitManager.Instance.RespawnHealthKit(this);
        }
    }
}
