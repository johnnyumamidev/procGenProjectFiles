using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    protected override void Update()
    {
        base.Update();
        if(playerInteraction != null)
        {
            transform.position = playerInteraction.weaponHoldPosition.position;
        }
    }
}
