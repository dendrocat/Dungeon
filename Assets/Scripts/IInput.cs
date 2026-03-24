using Vector2 = UnityEngine.Vector2;
using UnityEngine.Events;

public interface IInput
{
    public static IInput Instance { get; protected set; } = null;

    public Vector2 Move { get; }
    public bool IsCrouching { get; }
    public bool IsRunning { get; }

    public Vector2 MouseDelta { get; }
    public bool Attack { get; }

    public event UnityAction Jumped;
    public event UnityAction<int> WeaponNumed;
    public event UnityAction Reloaded;
    public event UnityAction Throwed;
    public event UnityAction MeleeAttacked;
}
