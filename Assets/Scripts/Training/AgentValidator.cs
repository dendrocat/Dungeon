using UnityEngine;
using Unity.MLAgents.Actuators;
using DomainLogging;

[RequireComponent(typeof(EnemyAgent))]
public class AgentValidator : MonoBehaviour, IActionReceiver
{
    [SerializeField] AgentValidatorConfig m_Config;

    EnemyAgent m_Agent;
    Enemy m_Enemy;

    void Awake()
    {
        m_Enemy = GetComponentInParent<Enemy>();
        m_Agent = m_Enemy.MLAgent;
        m_Agent.ActionReceived += OnActionReceived;
    }

    // float EstimateDie(States next)
    // {
    //     if (m_Enemy.Health.Value <= 0 && next == States.Die)
    //         return m_Config.Rewards.Correct + m_Config.Rewards.Die;
    //     return m_Config.Rewards.Incorrect;
    // }

    float EstimateAlert()
    {
        if (m_Agent.AudioSensor.AudioOutput.AudioLevel > m_Config.Detection.AudioLevel) return m_Config.Rewards.Correct;
        bool correct = Director.Instance.VisibilityChecker.IsPlayerVisibleFrom(m_Enemy) || Director.Instance.PlayerVisible;
        return correct ? m_Config.Rewards.Correct : m_Config.Rewards.Incorrect;
    }

    float EstimateAttack()
    {
        return Director.Instance.PlayerVisible ? m_Config.Rewards.Correct : m_Config.Rewards.Incorrect;
    }

    float EstimateCover()
    {
        if (m_Enemy.Health.Value / m_Enemy.Health.Max > 0.5f)
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

    float EstimateIdlePatrol()
    {
        // DomainDebug.Log($"EstimateIdlePatrol: PlayerVisible : {Director.Instance.PlayerVisible}", DomainType.Agent);
        if (Director.Instance.PlayerVisible)
            return m_Config.Rewards.Incorrect;
        return m_Config.Rewards.Correct;
    }

    public float EstimateAgentAction(States next)
    {
        // if (next == States.Die || m_Enemy.Health.Value <= 0)
        //     return EstimateDie(next);
        if (next == States.Alert) return EstimateAlert();
        if (next == States.Attack) return EstimateAttack();
        if (next == States.Cover) return EstimateCover();
        if (next == States.Search) return EstimateSearch();
        if (next == States.Idle || next == States.Patrol) return EstimateIdlePatrol();
        return m_Config.Rewards.Correct;
    }

    public void OnActionReceived(ActionBuffers actionBuffers)
    {
        States next = (States)actionBuffers.DiscreteActions[0];
        var rate = EstimateAgentAction(next);
        m_Agent.AddReward(rate);
        DomainDebug.Log($"Agent: {m_Enemy.name} getted {rate} for next state : {next}. All reward: {m_Agent.GetCumulativeReward()}", DomainType.Agent);
        if (m_Agent.GetCumulativeReward() <= m_Config.Rewards.Incorrect * 4)
        {
            m_Agent.EndEpisode();
            m_Enemy.gameObject.SetActive(false);
        }
    }

    public void WriteDiscreteActionMask(IDiscreteActionMask actionMask) { }
}
