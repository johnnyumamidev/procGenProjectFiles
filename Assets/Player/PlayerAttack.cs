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

    PlayerInteraction playerInteraction;
    PlayerInput playerInput;
    PlayerLocomotion playerLocomotion;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerLocomotion= GetComponent<PlayerLocomotion>(); 
    }

    public void HandleAllAttackActions()
    {
        HandlePlayerAttack();
        HandleRangedAttack();
    }
    bool boltFired;
    public float crossbowCooldownTime = 1f;
    public float verticalAimThreshold = 0.5f;
    float firingAngle;
    public Transform firingPointPivot;
    public Vector2 firingDirection;
    private void HandleRangedAttack()
    {
        firingAngle = 0;
        if (playerInput.aimDirection.normalized.y > verticalAimThreshold)
            firingAngle = 45f;
        else if (playerInput.aimDirection.normalized.y < 0)
            firingAngle = -45f;
        if (!playerLocomotion.facingRight) firingAngle *= -1;

        firingDirection = firingPoint.position - firingPointPivot.position;
        firingPointPivot.rotation = Quaternion.Euler(0,0,firingAngle);

        if(playerInput.performShoot != 0 && !boltFired)
        {
            fireProjectileEvent.Raise();
        }
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
    private void HandlePlayerAttack()
    {
        if (!playerInteraction.currentlyHoldingItem) return;

        if (playerInput.performAttack != 0)
        {
            attackActive.Raise();
        }
        else
        {
            attackInactive.Raise();
        }
    }
    private void OnEnable()
    {
        fireProjectileEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        fireProjectileEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        fireBolt?.Invoke();
    }
}
