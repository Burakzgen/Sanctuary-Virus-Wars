using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private PopupManager popupManager;

    [SerializeField] PlayfabManager m_PlayfabManager;
    [SerializeField] TextMeshProUGUI coinTitle;
    [SerializeField] int coinsPrice;
    private void Start()
    {
        if (PlayerPrefs.GetInt(itemName) == 1)
            this.gameObject.SetActive(false);

        purchaseButton.onClick.AddListener(OnPurchaseClicked);
        if (itemName == null)
            itemName = gameObject.name;

        coinTitle.text = $"{coinsPrice} $";
    }

    private void OnPurchaseClicked()
    {
        popupManager.ShowPopup($"Are you sure you want to purchase {itemName}?", "PURSCHASE ITEM", itemSprite, ConfirmPurchase);
    }

    private void ConfirmPurchase()
    {
        Debug.Log($"{itemName} purchased successfully!");
        BuyItem();
        SetItemPurchased(itemName, true);
        this.gameObject.SetActive(false);
    }
    public void SetItemPurchased(string itemName, bool isPurchased)
    {
        PlayerPrefs.SetInt(itemName, isPurchased ? 1 : 0);
    }
    void BuyItem()
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = coinsPrice
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnSubtractCoinsSuccess, OnError);
    }

    private void OnSubtractCoinsSuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Bought item! " + itemName);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSounds[0]);
        m_PlayfabManager.GetVirtualCurrencies();
    }
    private void OnError(PlayFabError error)
    {
        Debug.Log("Error :" + error.ErrorMessage);
    }
}
