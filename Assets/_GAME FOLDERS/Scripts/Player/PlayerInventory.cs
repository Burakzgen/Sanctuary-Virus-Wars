using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private bool hasAttackItem = false;
    public Transform attackItemPosition;

    private GameObject currentAttackItem;

    public void AddAttackItem(GameObject attackItem)
    {
        if (currentAttackItem != null)
        {
            Destroy(currentAttackItem);
        }

        hasAttackItem = true;
        currentAttackItem = Instantiate(attackItem, attackItemPosition.position, attackItemPosition.rotation);
        currentAttackItem.transform.SetParent(attackItemPosition);
    }

    public bool HasAttackItem()
    {
        return hasAttackItem;
    }
}
