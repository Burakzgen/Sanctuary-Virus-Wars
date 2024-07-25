using UnityEngine;

public class PoisonCure : MonoBehaviour, IInteractable
{
    public float cureAmount = 20f;

    public event System.Action OnCollected;
    public void Interact(PlayerHealth playerHealth)
    {
        if (playerHealth != null)
        {
            playerHealth.CurePoison(cureAmount);
            Destroy(gameObject);
        }
    }

}
