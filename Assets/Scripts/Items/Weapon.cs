using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : Item
{
    BoxCollider2D boxCollider;
    bool attackActive = false;
    public float attackActiveTime = 0.5f;

    private void Start()
    {
        EventManager.instance.AddListener("attack_active", AttackActive());
        EventManager.instance.AddListener("attack_inactive", AttackInactive());
    }
    protected override void Update()
    {
        base.Update();
        if(playerInteraction != null)
        {
            transform.position = playerInteraction.weaponHoldPosition.position;
            transform.rotation = playerInteraction.weaponHoldPosition.rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null && attackActive)
        {
            EventManager.instance.TriggerEvent(collision.name + "_take_damage");
        }
    }

    private UnityAction AttackActive()
    {
        UnityAction action = () =>
        {
            attackActive = true;
        };
        return action;
    }
    private UnityAction AttackInactive()
    {
        UnityAction action = () =>
        {
            attackActive = false;
        };
        return action;
    }
}
