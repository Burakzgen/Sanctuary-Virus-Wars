using UnityEngine;

public class HealthKit : MonoBehaviour, IInteractable
{
    [SerializeField] private float healthAmount = 15f;
    [SerializeField] bool useRandom = false;
    private float healthValue;

    public event System.Action OnCollected;
    public void Interact(PlayerHealth health)
    {
        if (health.CurrentHealth >= 100) return;

        if (health != null)
        {
            if (useRandom)
                healthValue = Random.Range(40, 60);
            else
                healthValue = healthAmount;

            health.Heal(healthValue);
            OnCollected?.Invoke();
        }
    }
}
