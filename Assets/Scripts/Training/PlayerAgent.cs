using UnityEngine;
using UnityEngine.Events;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent, IInput
{
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

    Player m_Player;

    public override void Initialize()
    {
        base.Initialize();
        IInput.Instance = this;
        m_Player = GetComponentInParent<Player>();
        Person.Died += OnPersonDied;
    }

    void OnPersonDied(Person p)
    {
        if (p is Enemy) AddReward(50f);
        else
        {
            AddReward(-100);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    { }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(m_Player.Health.Value / m_Player.Health.Max);
        sensor.AddObservation(m_Player.transform.position);
        sensor.AddObservation(m_Player.transform.forward);
        sensor.AddObservation(Director.Instance.PlayerLighted);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var inp = actions.ContinuousActions;
        Move = new Vector2(inp[0], inp[1]).normalized;
        MouseDelta = new Vector2(inp[2], 0);
        IsCrouching = inp[3] > 0;
        IsRunning = inp[4] > 0;
        Attack = inp[5] > 0;

        if (inp[6] > 0) Reloaded?.Invoke();
        if (inp[7] > 0) Throwed?.Invoke();
        if (inp[8] > 0) MeleeAttacked?.Invoke();
        WeaponNumed?.Invoke(Mathf.FloorToInt(3 * (inp[9] + 1) * 0.5f) + 1);
    }
}
