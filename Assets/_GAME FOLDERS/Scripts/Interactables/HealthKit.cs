using UnityEngine;

public class HealthKit : MonoBehaviour, IInteractable
{
    [SerializeField] private float healthAmount = 15f;
    PlayerHealth playerHealth;
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void Interact()
    {
        if (playerHealth.CurrentHealth >= 100) return;

        if (playerHealth != null)
        {
            playerHealth.Heal(healthAmount);
            HealthKitManager.Instance.RespawnHealthKit(this);
        }
    }
}
