using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour, IInput, IActivatable
{
    public bool IsActive => enabled;

    public Vector2 Move { get; private set; }
    public bool IsCrouching { get; private set; }
    public bool IsRunning { get; private set; }

    public Vector2 MouseDelta { get; private set; }

    public bool Attack { get; private set; }

    public event UnityAction Jumped;
    public event UnityAction<int> WeaponNumed;
    public event UnityAction Reloaded;
    public event UnityAction Throwed;
    public event UnityAction MeleeAttacked;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Move = ctx.ReadValue<Vector2>();
    }

    public void OnDelta(InputAction.CallbackContext ctx)
    {
        MouseDelta = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Jumped?.Invoke();
    }


    public void OnAttacked(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Attack = true;
        else if (ctx.canceled) Attack = false;
    }

    public void OnReloaded(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Reloaded?.Invoke();
    }

    public void OnWeaponNum(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        WeaponNumed?.Invoke(ctx.control.displayName[0] - '0');
    }

    public void OnThrow(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Throwed?.Invoke();
    }

    public void OnMelee(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) MeleeAttacked?.Invoke();
    }

    public void OnCrouch(InputAction.CallbackContext ctx) => IsCrouching = ctx.performed;
    public void OnRun(InputAction.CallbackContext ctx) => IsRunning = ctx.performed;

    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;
}
