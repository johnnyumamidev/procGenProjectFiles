using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
[CreateAssetMenu(menuName = "Shop/ItemData")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public int itemCost;
    public Texture itemSprite;
}
