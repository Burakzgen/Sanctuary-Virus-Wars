using UnityEngine;

public class EnergyCell : MonoBehaviour, IInteractable
{
    [SerializeField] Weapon[] weapons;

    public event System.Action OnCollected;
    public void Interact(PlayerHealth playerHealth)
    {
        int randomValue = Random.Range(0, weapons.Length);
        weapons[randomValue].damage = 50f;
        Destroy(gameObject);
    }
}
