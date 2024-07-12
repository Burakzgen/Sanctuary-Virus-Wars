using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private PopupManager popupManager;

    private void Start()
    {
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
        itemName = gameObject.name;
    }

    private void OnPurchaseClicked()
    {
        popupManager.ShowPopup($"Are you sure you want to purchase {itemName}?", "PURSCHASE ITEM", itemSprite, ConfirmPurchase);
    }

    private void ConfirmPurchase()
    {
        Debug.Log($"{itemName} purchased successfully!");
    }
}
