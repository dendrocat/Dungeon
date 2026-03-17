using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WaypointsProvider), typeof(EnemySpawner))]
public class Director : MonoBehaviour
{
    public event UnityAction<bool> PlayerVisibilityChanged;

    public static Director Instance { get; private set; }
    public WaypointsProvider WaypointsProvider { get; private set; }


    [SerializeField] Player m_Player;
    public Transform PlayerTransform => m_Player.transform;
    public Vector3? LastPlayerPos { get; private set; } = null;

    [SerializeField, Min(3)] float m_ChaseTime;

    bool m_PlayerVisible = false;
    public bool PlayerVisible
    {
        get => m_PlayerVisible;
        private set
        {
            if (m_PlayerVisible != value)
                PlayerVisibilityChanged?.Invoke(m_PlayerVisible);
            m_PlayerVisible = value;
        }
    }

    EnemySpawner m_Spawner;
    IReadOnlyList<Enemy> m_Enemies = null;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        WaypointsProvider = GetComponent<WaypointsProvider>();
        m_Spawner = GetComponent<EnemySpawner>();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void OnEnable()
    {
        m_Spawner.Spawned += OnEnemySpawned;
    }

    void OnDisable()
    {
        m_Spawner.Spawned -= OnEnemySpawned;
    }

    void OnEnemySpawned(IReadOnlyList<Enemy> enemies)
    {
        m_Enemies = enemies;
    }


    bool GetPlayerVisible(Enemy enemy)
    {
        if (enemy.MLAgent.RaySensor?.RayPerceptionOutput?.RayOutputs == null)
            return false;
        foreach (var outputs in enemy.MLAgent.RaySensor.RayPerceptionOutput.RayOutputs)
            if (outputs.HitTaggedObject) return true;
        return false;
    }

    void OnVisibilityChanged(bool playerVisibility)
    {
        if (playerVisibility) LastPlayerPos = null;
        else LastPlayerPos = m_Player.transform.position;
    }

    void FixedUpdate()
    {
        PlayerVisible = m_Enemies?.Any(GetPlayerVisible) ?? false;
    }

}
