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
        if (PlayerPrefs.GetInt(itemName) == 1)
            this.gameObject.SetActive(false);

        purchaseButton.onClick.AddListener(OnPurchaseClicked);
        if (itemName == null)
            itemName = gameObject.name;
    }

    private void OnPurchaseClicked()
    {
        popupManager.ShowPopup($"Are you sure you want to purchase {itemName}?", "PURSCHASE ITEM", itemSprite, ConfirmPurchase);
    }

    private void ConfirmPurchase()
    {
        Debug.Log($"{itemName} purchased successfully!");
        SetItemPurchased(itemName, true);
        this.gameObject.SetActive(false);
    }
    public void SetItemPurchased(string itemName, bool isPurchased)
    {
        PlayerPrefs.SetInt(itemName, isPurchased ? 1 : 0);
    }


}
