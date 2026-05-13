using UnityEngine;
using Unity.MLAgents.Actuators;
// using DomainLogging;

[RequireComponent(typeof(EnemyAgent))]
public class AgentValidator : MonoBehaviour
{
    [SerializeField] AgentRewards m_Rewards;

    EnemyAgent m_Agent;
    Enemy m_Enemy;

    void Awake()
    {
        m_Enemy = GetComponentInParent<Enemy>();
        m_Agent = m_Enemy.MLAgent;
        m_Agent.ActionReceived += OnActionReceived;

        Person.Died += OnDied;
    }

    void OnDied(Person p)
    {
        if (!m_Enemy.Equals(p)) return;
        m_Agent.AddReward(m_Rewards.Die);
    }

    float EstimateAlert()
    {
        if (m_Agent.AudioSensor.AudioOutput.AudioLevel > m_Enemy.Config.Detection.AudioLevel) return m_Rewards.Correct;
        bool correct = Director.Instance.VisibilityChecker.IsPlayerVisibleFrom(m_Enemy) || Director.Instance.PlayerVisible;
        return correct ? m_Rewards.Correct : m_Rewards.Incorrect;
    }

    float EstimateAttack()
    {
        return Director.Instance.PlayerVisible ? m_Rewards.Correct : m_Rewards.Incorrect;
    }

    float EstimateCover()
    {
        if (m_Enemy.Health.Value / m_Enemy.Health.Max > 0.5f)
            return m_Rewards.Incorrect;
        return m_Rewards.Correct;
    }

    float EstimateSearch()
    {
        if (Director.Instance.PlayerVisible)
            return m_Rewards.Incorrect;
        if (!Director.Instance.LastPlayerPos.HasValue)
            return m_Rewards.Incorrect;
        return m_Rewards.Correct;
    }

    float EstimateIdlePatrol()
    {
        // DomainDebug.Log($"EstimateIdlePatrol: PlayerVisible : {Director.Instance.PlayerVisible}", DomainType.Agent);
        if (Director.Instance.PlayerVisible)
            return m_Rewards.Incorrect;
        return m_Rewards.Correct;
    }

    float EstimateAgentAction(States next)
    {
        if (next == States.Alert) return EstimateAlert();
        if (next == States.Attack) return EstimateAttack();
        if (next == States.Cover) return EstimateCover();
        if (next == States.Search) return EstimateSearch();
        if (next == States.Idle || next == States.Patrol) return EstimateIdlePatrol();
        return m_Rewards.Correct;
    }

    void EndEpisode()
    {
        m_Agent.EndEpisode();
        m_Enemy.gameObject.SetActive(false);
    }

    public void OnActionReceived(ActionBuffers actionBuffers)
    {
        States next = (States)actionBuffers.DiscreteActions[0];
        var rate = EstimateAgentAction(next);
        m_Agent.AddReward(rate);
        // DomainDebug.Log($"Agent: {m_Enemy.name} getted {rate} for next state : {next}. All reward: {m_Agent.GetCumulativeReward()}", DomainType.Training);
        if (m_Agent.GetCumulativeReward() <= m_Rewards.Incorrect * 3)
            EndEpisode();
    }
}
