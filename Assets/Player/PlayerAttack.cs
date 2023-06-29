using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent attackActive;
    [SerializeField] GameEvent attackInactive;
    [SerializeField] GameEvent fireProjectileEvent;
    [SerializeField] UnityEvent fireBolt;

    public Transform firingPoint;
    public GameObject boltPrefab;

    [SerializeField] Transform currentWeapon;
    [SerializeField] Transform rangedWeapon;
    [SerializeField] Transform meleeWeapon;

    PlayerInteraction playerInteraction;
    PlayerInput playerInput;
    PlayerLocomotion playerLocomotion;

    public bool isMeleeAttacking;

    public float switchWeaponCooldown = 0.25f;
    bool switchWeaponReady = true;
    bool weaponsDisabled = false;
    private void Awake()
    {
        playerLocomotion= GetComponent<PlayerLocomotion>(); 
        playerInput = GetComponent<PlayerInput>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    public void HandleAllAttackActions()
    {
        AssignEquippedWeaponToSlot();

        CountTimeUntilSwitchWeaponReady();
        if (playerInput.performSwitch != 0 && switchWeaponReady) SwitchBetweenMeleeAndRangedWeapon();
        if (currentWeapon == meleeWeapon)
        {
            HandleMeleeAttack();
        }
        else
        {
            HandleRangedAttack();
        }
    }

    private void AssignEquippedWeaponToSlot()
    {
        meleeWeapon = playerInteraction.equippedMeleeWeapon.transform;
        rangedWeapon = playerInteraction.equippedRangedWeapon.transform;
        if (!weaponsDisabled)
        {
            meleeWeapon.GetComponent<SpriteRenderer>().enabled = false;
            rangedWeapon.GetComponent<SpriteRenderer>().enabled = false;
            weaponsDisabled = true;
        }

        if (currentWeapon == null)
        {
            currentWeapon = meleeWeapon;
            currentWeapon.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void CountTimeUntilSwitchWeaponReady()
    {
        if (!switchWeaponReady)
        {
            float timer = 0;
            timer += Time.deltaTime;
            if(timer >= switchWeaponCooldown)
            {
                switchWeaponReady = true;
            }
        }
    }

    private void SwitchBetweenMeleeAndRangedWeapon()
    {
        currentWeapon.GetComponent<SpriteRenderer>().enabled = false;
        if (rangedWeapon != null && currentWeapon != rangedWeapon)
        {
            Debug.Log("equipping: " + rangedWeapon);
            currentWeapon = rangedWeapon;
        }
        else if (meleeWeapon != null && currentWeapon != meleeWeapon)
        {
            Debug.Log("equipping: " + meleeWeapon);
            currentWeapon = meleeWeapon;
        }
        currentWeapon.GetComponent<SpriteRenderer>().enabled = true;
    }

    bool boltFired;
    public float crossbowCooldownTime = 1f;
    public float verticalAimThreshold = 0.5f;
    float firingAngle;
    public Transform firingPointPivot;
    public Transform weaponHoldPosition;
    public Vector2 firingDirection;
    private void HandleRangedAttack()
    {
        firingAngle = 0;
        float flipRotation = 0;
        if (playerInput.aimDirection.normalized.y > verticalAimThreshold)
            firingAngle = 45f;
        else if (playerInput.aimDirection.normalized.y < 0)
            firingAngle = -45f;
        if (!playerLocomotion.facingRight)
        {
            firingAngle *= -1;
            flipRotation = 180;
        }

        firingDirection = firingPoint.position - firingPointPivot.position;
        firingPointPivot.rotation = Quaternion.Euler(0,0,firingAngle);
        weaponHoldPosition.rotation = Quaternion.Euler(flipRotation, -flipRotation, -90 + firingAngle);
        rangedWeapon.rotation = weaponHoldPosition.rotation;
        if(playerInput.performShoot != 0 && !boltFired)
        {
            fireProjectileEvent.Raise();
        }

        if (isMeleeAttacking) return;
    }

    public void FireBolt()
    {
        GameObject bolt = Instantiate(boltPrefab, firingPoint.position, Quaternion.identity);
        BoltBehavior boltBehavior = bolt.GetComponent<BoltBehavior>();
        boltBehavior.boltDirection = firingDirection;
        float theta = Vector2.Angle(Vector2.up, firingDirection);
        if (playerLocomotion.facingRight) theta *= -1;
        bolt.transform.rotation = Quaternion.Euler(0, 0, theta);
        Debug.Log(bolt.transform.rotation.z);
        boltFired = true;
        StartCoroutine(CrossbowCooldown());
    }

    IEnumerator CrossbowCooldown()
    {
        while (boltFired)
        {
            yield return new WaitForSeconds(crossbowCooldownTime);
            boltFired = false;
        }
    }
    private void HandleMeleeAttack()
    {
        if (playerInput.performAttack != 0)
        {
            attackActive.Raise();
            isMeleeAttacking = true;
        }
        else
        {
            attackInactive.Raise();
            isMeleeAttacking = false;
        }
    }
    private void OnEnable()
    {
        fireProjectileEvent?.RegisterListener(this);
    }
    private void OnDisable()
    {
        fireProjectileEvent?.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        fireBolt?.Invoke();
    }
}
