using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Weapon/Data")]
public class WeaponData : ScriptableObject
{
    public bool isRangedWeapon;
    public int damage;
    public Sprite weaponSprite;
}
