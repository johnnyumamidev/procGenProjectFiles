using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    [Header("events")]
    [SerializeField] GameEvent buyItemEvent;
    [SerializeField] GameEvent updateCurrencyEvent;

    Button shopButton;

    [Header("item data")]
    public ShopItemData shopItemData;
    public TextMeshProUGUI shopItemPriceText;
    public RawImage shopItemSprite;
    public TextMeshProUGUI shopItemNameText;
    public int shopItemPrice;

    private void Start()
    {
        if(shopButton == null) shopButton = GetComponent<Button>();
        shopButton.onClick.AddListener(BuyItem);
    }

    public void BuyItem()
    {
        buyItemEvent.Raise();
        shopButton.gameObject.SetActive(false);
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        eventSystem.SetSelectedGameObject(GameObject.FindGameObjectWithTag("Shop"));
    }

    public UnityAction CheckPlayerCurrency(PlayerInventory playerInventory)
    {
        UnityAction action = () =>
        {
            if (playerInventory.currentCurrency < shopItemPrice)
            {
                Debug.Log("player cannot afford " + shopItemData.itemName);
            }
            else
            {
                Debug.Log(shopItemData.itemName + " bought, added 1 to inventory");
                for (int i = 1; i <= shopItemPrice; i++)
                {
                    updateCurrencyEvent.Raise();
                }
            }
        };
        return action;
    }
}
