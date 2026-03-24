using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using DomainLogging;

[RequireComponent(typeof(StateMachine))]
public class EnemyAgent : Agent, IActivatable
{
    [SerializeField] RayPerceptionSensorComponent3D m_RaySensorComponent;
    public RayPerceptionSensor RaySensor => m_RaySensorComponent.RaySensor;

    [SerializeField] AudioSensorComponent m_AudioSensorComponent;
    public AudioSensor AudioSensor => m_AudioSensorComponent.AudioSensor;

    public bool IsActive => enabled;

    StateMachine fsm;
    Enemy m_Enemy;

    public override void Initialize()
    {
        fsm = GetComponent<StateMachine>();
        fsm.ChangeStateRequested += RequestDecision;

		m_Enemy = GetComponentInParent<Enemy>();
    }

    void Start()
    {
        IInput.Instance.WeaponNumed += OnNum;
    }

    void OnNum(int state)
    {
        this.state = state - 1;
        // DomainDebug.Log($"On Numed: state {(States)this.state}", DomainType.Agent);
    }

    int state = 0;

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        DomainDebug.Log("Heuristic", DomainType.Agent);
        var d = actionsOut.DiscreteActions;
        d[0] = state;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Player position
        sensor.AddObservation(Vector3.Distance(Director.Instance.PlayerTransform.position, m_Enemy.transform.position));
        sensor.AddObservation(Vector3.Angle(Director.Instance.PlayerTransform.forward, m_Enemy.transform.forward));

        // Player visibility
        sensor.AddObservation(Director.Instance.PlayerVisible);
        sensor.AddObservation(Director.Instance.PlayerLighted);

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
        fsm.ChangeState(actions.DiscreteActions[0]);
    }

    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;
}
