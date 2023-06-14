using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : Item, IEventListener
{
    [SerializeField] GameEvent attackActiveEvent;
    [SerializeField] GameEvent attackInactiveEvent;
    [SerializeField] GameEvent playerFacingRight;
    [SerializeField] GameEvent playerFacingLeft;

    public LayerMask enemylayer;
    public WeaponData weaponData;
    SpriteRenderer spriteRenderer;

    bool attackActive = false;
    public float attackActiveTime = 0.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponData.weaponSprite;
        this.gameObject.name = weaponData.weaponType;
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
            GameEvent enemyDamageEvent = collision.GetComponent<EnemyHealth>().enemyDamageEvent;
            enemyDamageEvent.Raise();
        }
    }
    private void OnEnable()
    {
        attackActiveEvent.RegisterListener(this);
        attackInactiveEvent.RegisterListener(this);
        playerFacingRight.RegisterListener(this);
        playerFacingLeft.RegisterListener(this);
    }
    private void OnDisable()
    {
        attackActiveEvent.UnregisterListener(this);
        attackInactiveEvent.UnregisterListener(this);
        playerFacingRight.UnregisterListener(this);
        playerFacingLeft.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if (gameEvent == attackActiveEvent) attackActive = true;
        if (gameEvent == attackInactiveEvent) attackActive = false;
        if (gameEvent == playerFacingRight) spriteRenderer.sortingOrder = 2;
        if (gameEvent == playerFacingLeft) spriteRenderer.sortingOrder = 0;
    }
}
