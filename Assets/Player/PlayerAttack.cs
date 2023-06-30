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

    public Transform currentWeapon;
    [SerializeField] Transform rangedWeapon;
    [SerializeField] Transform meleeWeapon;

    PlayerInteraction playerInteraction;
    PlayerInput playerInput;
    PlayerLocomotion playerLocomotion;

    public bool isMeleeAttacking;

    public float switchWeaponCooldown = 0.25f;
    float switchWeaponTimer;
    public bool switchWeaponReady = true;
    bool weaponsEquipped = false;
    private void Awake()
    {
        switchWeaponTimer = switchWeaponCooldown;
        playerLocomotion = GetComponent<PlayerLocomotion>(); 
        playerInput = GetComponent<PlayerInput>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    public void HandleAllAttackActions()
    {
        AssignEquippedWeaponToSlot();
        if (playerInput.performSwitch != 0 && switchWeaponReady)
        {
            SwitchBetweenMeleeAndRangedWeapon();
        }

        if (!switchWeaponReady)
        {
            CountTimeUntilSwitchWeaponReady();
        }
        if (currentWeapon != rangedWeapon)
        {
            HandleMeleeAttack();
        }
        else
        {
            HandleRangedAttack();
        }
    }

    public void DropCurrentWeapon(GameObject newWeapon)
    {
        bool currentWeaponRanged = currentWeapon.GetComponent<Weapon>().weaponData.isRangedWeapon;
        bool newWeaponRanged = newWeapon.GetComponent<Weapon>().weaponData.isRangedWeapon;
        if (currentWeaponRanged != newWeaponRanged)
        {
            if (meleeWeapon != currentWeapon) meleeWeapon.GetComponent<SpriteRenderer>().enabled = true;
            else { rangedWeapon.GetComponent<SpriteRenderer>().enabled = true; }
            Debug.Log("picking up opposite weapon type");
        }
        currentWeapon = null;
        weaponsEquipped = false;
    }
    private void AssignEquippedWeaponToSlot()
    {
        meleeWeapon = playerInteraction.equippedMeleeWeapon.transform;
        rangedWeapon = playerInteraction.equippedRangedWeapon.transform;
        if (!weaponsEquipped)
        {
            meleeWeapon.GetComponent<SpriteRenderer>().enabled = false;
            rangedWeapon.GetComponent<SpriteRenderer>().enabled = false;
            if (currentWeapon == null)
            {
                currentWeapon = meleeWeapon;
                currentWeapon.GetComponent<SpriteRenderer>().enabled = true;
            }
            weaponsEquipped = true;
        }
    }

    private void CountTimeUntilSwitchWeaponReady()
    {
        switchWeaponTimer -= Time.deltaTime;
        if (switchWeaponTimer <= 0)
        {
            switchWeaponTimer = switchWeaponCooldown;
            switchWeaponReady = true;
        }
    }

    private void SwitchBetweenMeleeAndRangedWeapon()
    {
        if (!switchWeaponReady) return;
        currentWeapon.GetComponent<SpriteRenderer>().enabled = false;
        if (rangedWeapon != null && currentWeapon != rangedWeapon)
        {
            currentWeapon = rangedWeapon;
        }
        else if (meleeWeapon != null && currentWeapon != meleeWeapon)
        {
            currentWeapon = meleeWeapon;
        }
        currentWeapon.GetComponent<SpriteRenderer>().enabled = true;
        switchWeaponReady = false;
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
        if(gameEvent == fireProjectileEvent) fireBolt?.Invoke();
    }
}
