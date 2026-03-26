using UnityEngine;
using Unity.MLAgents.Actuators;
using UnityEngine.Events;
using DomainLogging;

[RequireComponent(typeof(EnemyAgent))]
public class AgentValidator : MonoBehaviour, IActionReceiver
{
    public static event UnityAction EpisodeEndingRequested;

    [SerializeField] AgentValidatorConfig m_Config;

    EnemyAgent m_Agent;
    Enemy m_Enemy;

    void Awake()
    {
        m_Enemy = GetComponentInParent<Enemy>();
        m_Agent = m_Enemy.MLAgent;
		m_Agent.ActionReceived += OnActionReceived;
    }

    float EstimateDie(States next)
    {
        if (next == States.Die && m_Enemy.Health.Value <= 0)
            return m_Config.Rewards.Correct + m_Config.Rewards.Die;
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
        var checkDist = !Director.Instance.PlayerLighted;
        foreach (var hit in m_Agent.RaySensor.RayPerceptionOutput.RayOutputs)
        {
            if (hit.HitTaggedObject)
            {
                if (checkDist && Vector3.Distance(hit.HitGameObject.transform.position, m_Enemy.transform.position) > m_Config.Detection.RayLength)
                    return m_Config.Rewards.Incorrect;
                return m_Config.Rewards.Correct;
            }
        }
        return m_Config.Rewards.Incorrect;
    }

    float EstimateCover()
    {
        if (m_Enemy.Health.RemainingHealCount <= 0)
            return m_Config.Rewards.Incorrect;
        return m_Config.Rewards.Correct;
    }

    float EstimateSearch()
    {
        if (Director.Instance.PlayerVisible)
            return m_Config.Rewards.Incorrect;
		if (!Director.Instance.LastPlayerPos.HasValue)
			return m_Config.Rewards.Incorrect;
        return m_Config.Rewards.Correct;
    }

	float EstimateIdlePatrol() {
		if (Director.Instance.PlayerVisible)
			return m_Config.Rewards.Incorrect;
		return m_Config.Rewards.Correct;
	}

    public float EstimateAgentAction(int nextState)
    {
        States next = (States)nextState;

        if (next == States.Die || m_Enemy.Health.Value <= 0)
            return EstimateDie(next);
        if (next == States.Alert) return EstimateAlert();
        if (next == States.Attack) return EstimateAttack();
        if (next == States.Cover) return EstimateCover();
        if (next == States.Search) return EstimateSearch();
		if (next == States.Idle || next == States.Patrol) return EstimateIdlePatrol();
        return m_Config.Rewards.Correct;
    }

    public void OnActionReceived(ActionBuffers actionBuffers)
    {
        var rate = EstimateAgentAction(actionBuffers.DiscreteActions[0]);
        m_Agent.AddReward(rate);
		DomainDebug.Log($"Agent: {m_Enemy.name} getted {rate}. All reward: {m_Agent.GetCumulativeReward()}", DomainType.Agent);
        if (rate == m_Config.Rewards.Incorrect)
            EpisodeEndingRequested?.Invoke();
    }

    public void WriteDiscreteActionMask(IDiscreteActionMask actionMask) { }
}
