using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DomainLogging;

[RequireComponent(typeof(WaypointsProvider), typeof(EnemySpawner))]
public class Director : MonoBehaviour
{
    public event UnityAction<bool> PlayerVisibilityChanged;

    public static Director Instance { get; private set; }
    public WaypointsProvider WaypointsProvider { get; private set; }

    [SerializeField] Player m_Player;
    public Transform PlayerTransform => m_Player.transform;
    public bool PlayerLighted => m_Player.IsLighted;
    public Vector3? LastPlayerPos { get; private set; } = null;
    [SerializeField] AgentValidatorConfig m_AgentConfig;

    bool m_PlayerVisible = false;
    public bool PlayerVisible
    {
        get => m_PlayerVisible;
        private set
        {
            if (m_PlayerVisible == value)
                return;
            m_PlayerVisible = value;
            PlayerVisibilityChanged?.Invoke(value);
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

        PlayerVisibilityChanged += OnVisibilityChanged;
    }
    public void SetPlayer(Player player)
    {
        m_Player = player;
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
        m_PlayerVisible = false;
        m_Enemies = enemies;
    }

    public bool IsPlayerVisibleFrom(Enemy enemy)
    {
		if (!enemy.gameObject.activeSelf) return false;
        var rays = enemy.MLAgent.RaySensor?.RayPerceptionOutput?.RayOutputs;
        if (rays == null) return false;
        foreach (var hit in rays)
        {
            if (hit.HitTaggedObject)
            {
                var dist = (hit.HitGameObject.transform.position - hit.StartPositionWorld).magnitude;
                // DomainDebug.Log($"Check ray out: {hit.HitTaggedObject} {dist / hit.ScaledRayLength}", DomainType.Director);
                if (PlayerLighted || dist / hit.ScaledRayLength <= m_AgentConfig.Detection.RayLengthScale)
                    return true;
            }
        }
        return false;
    }

    void OnVisibilityChanged(bool playerVisibility)
    {
        DomainDebug.Log($"PlayerVisibilityChanged to {m_PlayerVisible}", DomainType.Director);
        if (playerVisibility) LastPlayerPos = null;
        else LastPlayerPos = m_Player.transform.position;
    }

    void FixedUpdate()
    {
        PlayerVisible = m_Enemies?.Any(IsPlayerVisibleFrom) ?? false;
    }

}
