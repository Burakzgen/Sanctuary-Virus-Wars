using UnityEngine;

public class EnergyCell : MonoBehaviour, IInteractable
{
    [SerializeField] Weapon weapon;
    public void Interact()
    {
        weapon.damage = 50f;
        Destroy(gameObject);
    }
}
