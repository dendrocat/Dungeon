using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using InputContext = UnityEngine.InputSystem.InputAction.CallbackContext;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour, IInput, IActivatable
{
    public enum ActionMap { Player, UI }

    public bool IsActive => gameObject.activeInHierarchy;

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

    public event UnityAction Interacted;

    public event UnityAction Paused;

    PlayerInput m_Input;
    ActionMap m_CurrentMap = ActionMap.Player;

    void Awake()
    {
        m_Input = GetComponent<PlayerInput>();
        if (m_CurrentMap.ToString() != m_Input.currentActionMap.name)
            DomainLogging.DomainDebug.LogError($"The initial action map is incorrect", DomainLogging.DomainType.Player);
    }

    public void OnMove(InputContext ctx)
    {
        Move = ctx.ReadValue<Vector2>();
    }

    public void OnDelta(InputContext ctx)
    {
        MouseDelta = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputContext ctx)
    {
        if (ctx.performed) Jumped?.Invoke();
    }


    public void OnAttacked(InputContext ctx)
    {
        if (ctx.performed) Attack = true;
        else if (ctx.canceled) Attack = false;
    }

    public void OnReloaded(InputContext ctx)
    {
        if (ctx.performed) Reloaded?.Invoke();
    }

    public void OnWeaponNum(InputContext ctx)
    {
        if (!ctx.performed) return;
        WeaponNumed?.Invoke(ctx.control.displayName[0] - '0');
    }

    public void OnThrow(InputContext ctx)
    {
        if (ctx.performed) Throwed?.Invoke();
    }

    public void OnMelee(InputContext ctx)
    {
        if (ctx.performed) MeleeAttacked?.Invoke();
    }

    public void OnCrouch(InputContext ctx) => IsCrouching = ctx.performed;
    public void OnRun(InputContext ctx) => IsRunning = ctx.performed;

    public void OnInteracted(InputContext ctx)
    {
        if (ctx.performed) Interacted?.Invoke();
    }

    public void SwitchMap(ActionMap map)
    {
        m_Input.SwitchCurrentActionMap(map.ToString());
        m_CurrentMap = map;
    }
    public void OnPaused(InputContext ctx)
    {
        if (ctx.performed)
        {
            Paused?.Invoke();
            SwitchMap(m_CurrentMap == ActionMap.Player ? ActionMap.UI : ActionMap.Player);
        }
    }

    public void Activate() => gameObject.SetActive(true);
    public void Deactivate() => gameObject.SetActive(false);
}
