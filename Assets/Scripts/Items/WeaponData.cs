using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Weapon/Data")]
public class WeaponData : ScriptableObject
{
    public string weaponType;
    public int damage;
    public Sprite weaponSprite;
}
