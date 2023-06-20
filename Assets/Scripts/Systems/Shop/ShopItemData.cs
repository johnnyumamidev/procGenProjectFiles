using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Shop/ItemData")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public int itemCost;
    public Texture itemSprite;
}
