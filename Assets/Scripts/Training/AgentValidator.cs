using UnityEngine;
using Unity.MLAgents.Actuators;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyAgent))]
public class AgentValidator : MonoBehaviour, IActionReceiver
{
    public static event UnityAction EpisodeEndingRequested;

    [SerializeField] AgentValidatorConfig m_Config;

    EnemyAgent m_Agent;
    Enemy m_Enemy;

    public bool ShouldEndEpisode { get; private set; } = false;

    void Awake()
    {
        m_Enemy = GetComponentInParent<Enemy>();
        m_Agent = m_Enemy.MLAgent;
    }

    float EstimateDie(States next)
    {
        if (next == States.Die)
            return m_Config.Rewards.Correct + m_Config.Rewards.Die;
        ShouldEndEpisode = true;
        return m_Config.Rewards.Incorrect;
    }

    float EstimateAlert()
    {
        if (m_Agent.AudioSensor.AudioOutput.AudioLevel > m_Config.Detection.AudioLevel) return m_Config.Rewards.Correct;
        if (!Director.Instance.PlayerLighted)
        {
            foreach (var hit in m_Agent.RaySensor.RayPerceptionOutput.RayOutputs)
            {
                if (hit.HitTaggedObject)
                {
                    if (Vector3.Distance(hit.HitGameObject.transform.position, m_Enemy.transform.position) > m_Config.Detection.RayLength)
                        return m_Config.Rewards.Incorrect;
                    break;
                }
            }
        }
        return m_Config.Rewards.Correct;
    }

    float EstimateAttack()
    {
        return m_Config.Rewards.Correct;
    }

    float EstimateCover()
    {
        return m_Config.Rewards.Correct;
    }

    public float EstimateAgentAction(int nextState)
    {
        States next = (States)nextState;

        if (m_Enemy.Health.Value <= 0) return EstimateDie(next);

        if (next == States.Alert) return EstimateAlert();
        if (next == States.Attack) return EstimateAttack();
        if (next == States.Cover) return EstimateCover();
        return m_Config.Rewards.Correct;
    }

    public void OnActionReceived(ActionBuffers actionBuffers)
    {
        EstimateAgentAction(actionBuffers.DiscreteActions[0]);
    }

    public void WriteDiscreteActionMask(IDiscreteActionMask actionMask) { }
}
