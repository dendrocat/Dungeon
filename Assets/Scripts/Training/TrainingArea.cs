using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TriInspector;
using DomainLogging;
using System.Linq;

public class TrainingArea : MonoBehaviour
{
    [SerializeField, Slider(60, 7200)] float m_MaxTrainTime = 600;
    [SerializeField] AgentRewards m_Rewards;
    [SerializeField] EnemySpawner m_Spawner;
    [SerializeField] Player m_PlayerPrefab = null;
    Player m_Player = null;
    [SerializeField] Vector2 m_PlayerSpawnZone;
    [SerializeField] LayerMask m_ObstacleMask;

    [AssetsOnly]
    [SerializeField] Room[] m_TrainRooms = null;
    Room m_TrainRoom = null;

    SimpleMultiAgentGroup m_Group;
    HashSet<Enemy> m_Enemies;

    int m_Died;
    Timer m_TrainTimer;

    void Awake()
    {
        Academy.Instance.OnEnvironmentReset += OnEnvironmentReseted;
        m_Spawner.Spawned += RegisterEnemies;

        m_TrainTimer = new Timer(m_MaxTrainTime, false);

        // AgentValidator.EpisodeEndingRequested += OnEndEpisodeRequested;
#if UNITY_EDITOR || TRAIN
        DomainDebug.Log($"I'm training", DomainType.Training);
#endif
    }

    void OnDisable()
    {
        // DomainDebug.LogWarning($"Disabling training area", DomainType.Training);
        Person.Died -= OnPersonDied;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.tan;
        Vector3 size = Vector3.up;
        size.x = m_PlayerSpawnZone.x;
        size.z = m_PlayerSpawnZone.y;
        Gizmos.DrawCube(transform.position + Vector3.up, size);
    }

    [DisableInEditMode]
    [Button]
    void SetRoom()
    {
        var prefab = m_TrainRooms[Random.Range(0, m_TrainRooms.Length)];
        m_TrainRoom = Instantiate(prefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Room>();
        m_TrainRoom.PlayerExited += EndEpisode;
        DomainDebug.Log($"Setting room", DomainType.Training);
    }

    [DisableInEditMode]
    [Button]
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
        if (m_PlayerPrefab != null)
        {
            DomainDebug.Log($"Setting player to {nPos}", DomainType.Training);
            m_Player = Instantiate(m_PlayerPrefab.gameObject, nPos, Quaternion.identity).GetComponent<Player>();
            Director.Instance.SetPlayer(m_Player);
        }
    }

    [DisableInEditMode]
    [Button]
    void ClearArea()
    {
        if (m_TrainRoom != null) Destroy(m_TrainRoom.gameObject);
        if (m_Player != null) Destroy(m_Player.gameObject);
        if (m_Group != null)
        {
            m_Group.Dispose();
            // foreach (var enemy in m_Enemies)
            // {
            //     enemy.TakeDamage(10000);
            //     Destroy(enemy.gameObject);
            // }
            m_Enemies.Clear();
        }
        foreach (var obj in FindObjectsByType<Ammo>(FindObjectsSortMode.None))
        {
            Destroy(obj.gameObject);
        }
        DomainDebug.Log("Area cleared", DomainType.Training);
    }


    [DisableInEditMode]
    [Button("Reset Environment")]
    void OnEnvironmentReseted()
    {
        DomainDebug.LogWarning($"EnvironmentReseting", DomainType.Training);
        Person.Died -= OnPersonDied;
        ClearArea();

        SetRoom();
        SetPlayer();

        m_TrainTimer.Reset();
        m_TrainTimer.Activate();

        m_Died = 0;
        Person.Died += OnPersonDied;
    }

    void RegisterEnemies(IReadOnlyCollection<Enemy> enemies)
    {
        m_Enemies = enemies.ToHashSet();
        m_Group = new SimpleMultiAgentGroup();
        foreach (var enemy in enemies)
            m_Group.RegisterAgent(enemy.MLAgent);
        DomainDebug.Log($"Registered {enemies.Count}. All agent in group: {m_Group.GetRegisteredAgents().Count}", DomainType.Training);
    }

    void OnPersonDied(Person p)
    {
        if (p is Player) OnPlayerDied();
        if (p is Enemy e) OnEnemyDied(e);
    }

    void OnPlayerDied()
    {
        m_Group.AddGroupReward(m_Rewards.PlayerKill);
        DomainDebug.Log($"Player killed", DomainType.Training);
        EndEpisode();
    }

    void OnEnemyDied(Enemy e)
    {
        // if (!m_Enemies.Contains(e)) return;
        ++m_Died;
        e.MLAgent.AddReward(m_Rewards.Die);
        m_Group.UnregisterAgent(e.MLAgent);
        m_Enemies.Remove(e);
        DomainDebug.Log($"Died {e.name}. Remaining agents: {m_Enemies.Count}. Time elapsed: {m_TrainTimer.Progress * m_MaxTrainTime}", DomainType.Training);
        if (m_Group.GetRegisteredAgents().Count > 0) return;
        EndEpisode();
    }

    void OnTimerEnded()
    {
        m_Group.AddGroupReward(m_Rewards.TimeEnd);
        DomainDebug.Log($"Time ended.", DomainType.Training);
        EndEpisode();

    }
    // void OnEndEpisodeRequested()
    // {
    //     ++m_Requests;
    //     DomainDebug.Log($"OnEndEpisodeRequested, remaining {m_Group.GetRegisteredAgents().Count}", DomainType.Training);
    //     if (m_Group.GetRegisteredAgents().Count > 0) return;
    //     EndEpisode();
    // }

    void EndEpisode()
    {
        DomainDebug.LogWarning($"Episode ended\nDied: {m_Died}.\nPlayer killed: {m_Player.Health.Value <= 0}.\nTime elapsed: {m_TrainTimer.Progress * m_MaxTrainTime}", DomainType.Training);
        m_Group.EndGroupEpisode();
        m_Player?.GetComponentInChildren<Agent>()?.EndEpisode();
        OnEnvironmentReseted();
    }

    void OnApplicationQuit()
    {
        Person.Died -= OnPersonDied;
        // DomainDebug.LogWarning($"Application quiting", DomainType.Training);
    }

    void FixedUpdate()
    {
        m_TrainTimer.Update(Time.fixedDeltaTime);
    }

}
