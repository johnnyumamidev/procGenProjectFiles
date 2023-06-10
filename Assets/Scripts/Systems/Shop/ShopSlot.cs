using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    public ShopItemData shopItemData;
    public TextMeshProUGUI shopItemPriceText;
    public RawImage shopItemSprite;
    public TextMeshProUGUI shopItemNameText;
    public int shopItemPrice;

    public void BuyItem()
    {
        for(int i = 1; i <= shopItemPrice; i++)
        {
            EventManager.instance.TriggerEvent("decrease_currency");
        }
    }
}
