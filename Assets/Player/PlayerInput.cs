using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputActions;
    InputAction uiCancel;
    InputAction movement;
    InputAction jump;
    InputAction interact;
    InputAction attack;
    InputAction aim;
    InputAction shoot;

    public float performCancel;
    public Vector2 movementInput;
    public float performJump;
    public float performInteract;
    public float performAttack;
    public Vector2 aimDirection;
    public float performShoot;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        //UI
        uiCancel = inputActions.UI.Cancel;
        uiCancel.Enable();
        //gameplay
        movement = inputActions.Player.Move;
        jump = inputActions.Player.Jump;
        interact = inputActions.Player.Interact;
        attack = inputActions.Player.Attack;
        aim = inputActions.Player.Aim;
        shoot = inputActions.Player.Shoot;
        movement.Enable();
        jump.Enable();
        interact.Enable();
        attack.Enable();
        aim.Enable();
        shoot.Enable();
    }

    private void OnDisable()
    {
        uiCancel.Disable();

        movement.Disable();
        jump.Disable();
        interact.Disable();
        attack.Disable();
        aim.Disable();
        shoot.Disable();
    }

    public void HandleAllInputs()
    {
        performCancel = uiCancel.ReadValue<float>();

        movementInput = movement.ReadValue<Vector2>();
        performJump = jump.ReadValue<float>();
        performInteract = interact.ReadValue<float>();
        performAttack = attack.ReadValue<float>();
        aimDirection = aim.ReadValue<Vector2>();
        performShoot = shoot.ReadValue<float>();
    }
}
