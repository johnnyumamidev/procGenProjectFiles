using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWeapon : MonoBehaviour
{
    public Transform weaponHold;
    public Transform animationTarget;
    private void Update()
    {
        weaponHold.position = animationTarget.position;
        weaponHold.rotation = animationTarget.rotation;
    }
}
