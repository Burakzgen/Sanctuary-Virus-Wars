using UnityEngine;

public class HealthKit : MonoBehaviour, IInteractable
{
    [SerializeField] private float healthAmount = 15f;
    public void Interact(PlayerHealth health)
    {
        if (health.CurrentHealth >= 100) return;

        if (health != null)
        {
            health.Heal(healthAmount);
            HealthKitManager.Instance.RespawnHealthKit(this);
        }
    }
}
