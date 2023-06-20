using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Player/DataSO")]
public class PlayerData : ScriptableObject
{
    [Header("Health")]
    public float maxPlayerHealth;

    [Header("Hurt State")]
    public float hurtStateTime = 1;
    public float hurtLaunchForce = 0.01f;

    [Header("Rigidbody Properties")]
    public float playerGravityScale = 3;
    public float playerMass = 4;

    [Header("Movement Properties")]
    public float maxSpeed = 3;
    public float moveSpeed = 3;
    public float jumpForce = 3;
    public float dodgeForce = 3;

    [Header("Grounded Checks")]
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public Vector2 groundCheckOffset;

    [Header("Jump Properties")]
    public int maxJumps = 1;
    public float coyoteTime = 0.3f;
    public float landingRecoveryTimer = 0;
    public float jumpCooldown = 0.5f;
    public float aerialDriftModifier;
    public float maxAerialSpeed;
    public float jumpReleaseTimeAllowance = 0.1f;

    [Header("Climbing Properties")]
    public float climbSpeed;
    public float relaseChainForce = 0.01f;
}
