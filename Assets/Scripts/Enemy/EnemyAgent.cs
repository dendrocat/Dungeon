using UnityEngine;
using UnityEngine.Events;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
// using DomainLogging;

[RequireComponent(typeof(StateMachine))]
public class EnemyAgent : Agent
{
    public event UnityAction<ActionBuffers> ActionReceived;

    [SerializeField] RayPerceptionSensorComponent3D m_RaySensorComponent;
    public RayPerceptionSensor RaySensor => m_RaySensorComponent.RaySensor;

    [SerializeField] AudioSensorComponent m_AudioSensorComponent;
    public AudioSensor AudioSensor => m_AudioSensorComponent.AudioSensor;

    [SerializeField, Min(0.1f)] float m_UpdateSensorTime = 1;
    Timer m_UpdateTimer;

    StateMachine fsm;
    Enemy m_Enemy;

    public override void Initialize()
    {
        fsm = GetComponent<StateMachine>();
        fsm.ChangeStateRequested += RequestDecision;

        m_Enemy = GetComponentInParent<Enemy>();

        m_UpdateTimer = new Timer(m_UpdateSensorTime);
        m_UpdateTimer.TimerEnded += UpdateSensors;
        UpdateSensors();

        Person.Died += OnDied;
        Director.Instance.PlayerVisibilityChanged += OnVisibilityChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        fsm.ChangeStateRequested -= RequestDecision;
        Person.Died -= OnDied;
    }

    // void OnNum(int state)
    // {
    //     this.state = state - 1;
    //     // DomainDebug.Log($"On Numed: state {(States)this.state}", DomainType.Agent);
    // }
    //
    // int state = 0;
    //
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // DomainDebug.Log("Heuristic", DomainType.Agent);
        // var d = actionsOut.DiscreteActions;
        // d[0] = state;
    }

    void OnVisibilityChanged(bool _)
    {
        // DomainDebug.Log("Player Visibility received", DomainType.Agent);
        RequestDecision();
    }

    void OnDied(Person p)
    {
        if (!m_Enemy.Equals(p)) return;

        Director.Instance.PlayerVisibilityChanged -= OnVisibilityChanged;

        fsm.ChangeStateRequested -= RequestDecision;
        fsm.ChangeState((int)States.Die);

        EndEpisode();
        enabled = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // // Player position
        // sensor.AddObservation(Vector3.Distance(Director.Instance.PlayerTransform.position, m_Enemy.transform.position));
        // sensor.AddObservation(Vector3.Angle(Director.Instance.PlayerTransform.forward, m_Enemy.transform.forward));

        // Player visibility
        sensor.AddObservation(Director.Instance.VisibilityChecker.IsPlayerVisibleFrom(m_Enemy));
        sensor.AddObservation(Director.Instance.PlayerVisible);
        sensor.AddObservation(Director.Instance.Player.IsLighted);
        // DomainDebug.Log($"{m_Enemy.name} {Director.Instance.VisibilityChecker.IsPlayerVisibleFrom(m_Enemy)} {Director.Instance.PlayerVisible} {Director.Instance.Player.IsLighted}", DomainType.Agent);

        // Health
        sensor.AddObservation(m_Enemy.Health.Value / m_Enemy.Health.Max);

        // FSM state
        sensor.AddOneHotObservation(fsm.GetActiveState(), 7);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        var exits = fsm.GetExitStates();
        for (int i = 0; i < exits.Count; ++i)
            actionMask.SetActionEnabled(0, i, exits[i]);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // DomainDebug.Log($"{m_Enemy.name} (enabled : {enabled}) getted next state: {(States)actions.DiscreteActions[0]}", DomainType.Agent);
        fsm.ChangeState(actions.DiscreteActions[0]);
        ActionReceived?.Invoke(actions);
    }

    float prevAudio = 0f;
    void CheckAudio()
    {
        float audio = AudioSensor?.AudioOutput.AudioLevel ?? 0;
        if (audio > m_Enemy.Config.Detection.AudioLevel
                && prevAudio < m_Enemy.Config.Detection.AudioLevel)
            RequestDecision();
        prevAudio = audio;
    }

    void UpdateSensors()
    {
        RaySensor?.Update();
        AudioSensor?.Update();
        CheckAudio();

        m_UpdateTimer.Reset();
        m_UpdateTimer.Activate();
    }

    void FixedUpdate()
    {
        m_UpdateTimer.Update(Time.fixedDeltaTime);
    }
}
