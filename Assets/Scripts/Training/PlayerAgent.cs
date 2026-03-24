using UnityEngine;
using Unity.MLAgents;
using UnityEngine.Events;
using Unity.MLAgents.Actuators;

public class PlayerAgent : Agent, IInput
{
    public IInput Instance { get; private set; } = null;

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

    public override void Initialize()
    {
        base.Initialize();
		Instance = this;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
		var inp = actions.ContinuousActions;
		Move = new Vector2((inp[0] - 0.5f) * 2, (inp[1] - 0.5f) * 2).normalized;
		MouseDelta = new Vector2((inp[2] - 0.5f) * 2, (inp[3] - 0.5f) * 2).normalized;
		IsCrouching = inp[4] > 0.5f;
		IsRunning = inp[5] > 0.5f;
		Attack = inp[6] > 0.5f;

		if (inp[7] > 0.5f) Jumped?.Invoke();
		if (inp[8] > 0.5f) Reloaded?.Invoke();
		if (inp[9] > 0.5f) Throwed?.Invoke();
		if (inp[10] > 0.5f) MeleeAttacked?.Invoke();
		WeaponNumed?.Invoke(Mathf.FloorToInt(3 * inp[11]));
    }
}
