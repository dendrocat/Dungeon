using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using DomainLogging;

public class TrainingArea : MonoBehaviour
{
    [SerializeField] AgentValidatorConfig m_Config;
    [SerializeField] EnemySpawner m_Spawner;
    [SerializeField] Player m_PlayerPrefab = null;
    Player m_Player = null;
    [SerializeField] Vector2 m_PlayerSpawnZone;
    [SerializeField] LayerMask m_ObstacleMask;

    SimpleMultiAgentGroup m_Group;
    IReadOnlyCollection<Enemy> m_Enemies;

    int m_DiedEnemies = 0;

    void Awake()
    {
        Academy.Instance.OnEnvironmentReset += OnEnvironmentReseted;
        m_Spawner.Spawned += RegisterEnemies;
        Player.Died += OnPlayerDied;
        Enemy.Died += OnEnemyDied;

        AgentValidator.EpisodeEndingRequested += OnEndEpisodeRequested;
    }

    void SetPlayer()
    {
        Vector3 nPos = Vector3.positiveInfinity;
        while (nPos.Equals(Vector3.positiveInfinity))
        {
            nPos = Vector3.up;
            nPos.x = Random.Range(-m_PlayerSpawnZone.x / 2, m_PlayerSpawnZone.x / 2);
            nPos.z = Random.Range(-m_PlayerSpawnZone.y / 2, m_PlayerSpawnZone.y / 2);

            // DomainDebug.Log($"Try pos: {nPos}", DomainType.Training);
            if (Physics.CheckSphere(nPos, 2, m_ObstacleMask))
                nPos = Vector3.positiveInfinity;
        }
        // DomainDebug.Log($"Setting player to {nPos}", DomainType.Training);
        if (m_Player != null)
        {
            Destroy(m_Player.gameObject);
        }
        if (m_PlayerPrefab != null)
        {
            m_Player = Instantiate(m_PlayerPrefab.gameObject, nPos, Quaternion.identity).GetComponent<Player>();
            Director.Instance.SetPlayer(m_Player);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.tan;
        Vector3 size = Vector3.up;
        size.x = m_PlayerSpawnZone.x;
        size.z = m_PlayerSpawnZone.y;
        Gizmos.DrawCube(transform.position + Vector3.up, size);
    }

    void OnEnvironmentReseted()
    {
        // DomainLogging.DomainDebug.Log($"EnvironmentReseted", DomainType.Training);
        if (m_Enemies != null)
        {
            m_Group.Dispose();
            foreach (var enemy in m_Enemies)
            {
                Destroy(enemy.gameObject);
            }
        }
        foreach (var obj in FindObjectsByType<Ammo>(FindObjectsSortMode.None))
        {
            Destroy(obj.gameObject);
        }
        SetPlayer();
        m_DiedEnemies = 0;
    }

    void RegisterEnemies(IReadOnlyList<Enemy> enemies)
    {
        m_Enemies = enemies;
        m_Group = new SimpleMultiAgentGroup();
        foreach (var enemy in enemies)
            m_Group.RegisterAgent(enemy.MLAgent);
        DomainDebug.Log($"Registered {enemies.Count}. All agent in group: {m_Group.GetRegisteredAgents().Count}", DomainType.Training);
    }

    void OnPlayerDied()
    {
        m_Group.AddGroupReward(m_Config.Rewards.PlayerKill);
        EndEpisode();
    }

    void OnEnemyDied()
    {
        ++m_DiedEnemies;
        if (m_DiedEnemies != m_Enemies.Count) return;
        m_Group.AddGroupReward(m_Config.Rewards.GroupDie);
        EndEpisode();
    }

    void OnEndEpisodeRequested()
    {
        DomainDebug.Log($"OnEndEpisodeRequested: {m_Group.GetRegisteredAgents().Count}:{m_Enemies.Count}", DomainType.Training);
        if (m_Group.GetRegisteredAgents().Count > 0) return;
        EndEpisode();
    }

    void EndEpisode()
    {
        DomainDebug.LogWarning($"Episode ended\nDied: {m_DiedEnemies}\n\nPlayerDied: {m_Player.Health.Value == 0}", DomainType.Training);
        m_Group.EndGroupEpisode();
        m_Player.GetComponentInChildren<Agent>()?.EndEpisode();
        OnEnvironmentReseted();
    }

}
