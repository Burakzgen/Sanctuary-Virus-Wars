using UnityEngine;

public class AttackItem : MonoBehaviour, IInteractable
{

    public event System.Action OnCollected;
    public void Interact(PlayerHealth playerHealth)
    {
        PlayerInventory playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory != null)
        {
            playerInventory.AddAttackItem(gameObject);
            gameObject.SetActive(false);
        }
    }
}
