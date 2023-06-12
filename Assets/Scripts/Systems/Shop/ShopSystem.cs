using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public List<GameObject> shopSlots = new List<GameObject>();
    public List<ShopItemData> shopItems = new List<ShopItemData>();

    void Start()
    {
        foreach(GameObject slot in shopSlots)
        {
            ShopSlot shopSlot = slot.GetComponent<ShopSlot>();
            int itemIndex = Random.Range(0, shopItems.Count - 1);
            ShopItemData itemData = shopItems[itemIndex];
            shopSlot.shopItemData = itemData;
            shopSlot.shopItemPriceText.text = "$$" + itemData.itemCost.ToString();
            shopSlot.shopItemPrice = itemData.itemCost;
            shopSlot.shopItemSprite.texture = itemData.itemSprite;
            shopSlot.shopItemNameText.text = itemData.itemName;
            shopItems.Remove(itemData);
        }
    }
}
