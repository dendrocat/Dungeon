using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using DomainLogging;

public class TrainingArea : MonoBehaviour
{
    [SerializeField] AgentValidatorConfig m_Config;
    [SerializeField] EnemySpawner m_Spawner;

    SimpleMultiAgentGroup m_Group;

    int m_DiedEnemies = 0;

    void Awake()
    {
        Academy.Instance.OnEnvironmentReset += OnEnvironmentReseted;
        m_Spawner.Spawned += RegisterEnemies;
        Player.Died += OnPlayerDied;
        Enemy.Died += OnEnemyDied;
    }

    void OnEnvironmentReseted()
    {
        DomainLogging.DomainDebug.Log($"Test reseting", DomainType.Training);
        m_DiedEnemies = 0;
    }

    void RegisterEnemies(IReadOnlyList<Enemy> enemies)
    {
        m_Group = new SimpleMultiAgentGroup();
        foreach (var enemy in enemies)
            m_Group.RegisterAgent(enemy.MLAgent);
    }

    void OnPlayerDied()
    {
        m_Group.AddGroupReward(m_Config.Rewards.PlayerKill);
        m_Group.EndGroupEpisode();
    }

    void OnEnemyDied()
    {
        ++m_DiedEnemies;
        if (m_DiedEnemies != m_Group.GetRegisteredAgents().Count) return;
		m_Group.AddGroupReward(m_Config.Rewards.GroupDie);
		m_Group.EndGroupEpisode();
    }

}
