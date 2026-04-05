using UnityEngine;
using UnityEngine.Events;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent, IInput, IActivatable
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

    [SerializeField] PlayerWeaponHandler m_WeaponHandler;
    [SerializeField] GrenadeStats m_GrenadeStats;

    Player m_Player;
    Timer m_GrenadeWindow;

    public override void Initialize()
    {
        base.Initialize();
        m_Player = GetComponentInParent<Player>();
        Person.Died += OnPersonDied;

        m_GrenadeWindow = new Timer(m_GrenadeStats.ExplosionTime * 1.1f, false);
    }

    void OnPersonDied(Person p)
    {
        if (p is Enemy) AddReward(50f);
        else
        {
            if (m_GrenadeWindow.IsActive)
                AddReward(-300);
            else
                AddReward(-100);
            DomainLogging.DomainDebug.Log($"Player died with reward: {GetCumulativeReward()}", DomainLogging.DomainType.Player);
            Person.Died -= OnPersonDied;
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
        if (inp[7] > 0)
        {
            Throwed?.Invoke();
            if (!m_WeaponHandler.Grenade.IsReloading)
            {
                m_GrenadeWindow.Reset();
                m_GrenadeWindow.Activate();
            }
        }

        if (inp[8] > 0) MeleeAttacked?.Invoke();
        WeaponNumed?.Invoke(Mathf.FloorToInt(3 * (inp[9] + 1) * 0.5f) + 1);
    }

    void FixedUpdate()
    {
        m_GrenadeWindow.Update(Time.fixedDeltaTime);
    }

    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;
}
