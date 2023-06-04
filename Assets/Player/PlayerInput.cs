using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputActions;
    InputAction movement;
    InputAction jump;
    InputAction interact;
    InputAction attack;

    public Vector2 movementInput;
    public float performJump;
    public float performInteract;
    public float performAttack;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        movement = inputActions.Player.Move;
        jump = inputActions.Player.Jump;
        interact = inputActions.Player.Interact;
        attack = inputActions.Player.Attack;
        movement.Enable();
        jump.Enable();
        interact.Enable();
        attack.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        interact.Disable();
        attack.Disable();
    }

    public void HandleAllInputs()
    {
        movementInput = movement.ReadValue<Vector2>();
        performJump = jump.ReadValue<float>();
        performInteract = interact.ReadValue<float>();
        performAttack = attack.ReadValue<float>();
    }
}
