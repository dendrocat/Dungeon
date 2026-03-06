using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } = null;

    public Vector2 Move { get; private set; }
    public Vector2 MouseDelta { get; private set; }
    public bool Attack { get; private set; }

    [HideInInspector] public UnityEvent Jumped = new();
    [HideInInspector] public UnityEvent<int> WeaponNumed = new();
    [HideInInspector] public UnityEvent Reloaded = new();
    [HideInInspector] public UnityEvent Throwed = new();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

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
        if (ctx.performed) Jumped.Invoke();
    }


    public void OnAttacked(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Attack = true;
        else if (ctx.canceled) Attack = false;
    }

    public void OnReloaded(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Reloaded.Invoke();
    }

    public void OnWeaponNum(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        WeaponNumed.Invoke(ctx.control.displayName[0] - '0');
    }

    public void OnThrow(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Throwed.Invoke();
    }
}
