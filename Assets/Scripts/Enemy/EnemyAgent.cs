using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(StateMachine))]
public class EnemyAgent : Agent, IActivatable
{
    [SerializeField] RayPerceptionSensorComponent3D m_RaySensorComponent;
    public RayPerceptionSensor RaySensor => m_RaySensorComponent.RaySensor;

    [SerializeField] AudioSensorComponent m_AudioSensorComponent;
    public AudioSensor AudioSensor => m_AudioSensorComponent.AudioSensor;

    public bool IsActive => enabled;

    StateMachine fsm;

    public override void Initialize()
    {
        fsm = GetComponent<StateMachine>();
        fsm.ChangeStateRequested += RequestDecision;
    }

    void Start()
    {
        InputManager.Instance.WeaponNumed += OnNum;
    }

    void OnNum(int state)
    {
        this.state = state - 1;
        Debug.Log($"On Numed: state {(States)this.state}");
    }

    int state = 0;

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log("Heuristic");
        var d = actionsOut.DiscreteActions;
        d[0] = state;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        Debug.Log($"{RaySensor == null}");
        Debug.Log($"{AudioSensor == null}");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        fsm.ChangeState(actions.DiscreteActions[0]);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        var exits = fsm.GetExitStates();
        for (int i = 0; i < exits.Count; ++i)
            actionMask.SetActionEnabled(0, i, exits[i]);
    }

    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;
}
