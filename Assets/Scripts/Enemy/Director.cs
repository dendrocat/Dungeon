using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DomainLogging;

[RequireComponent(typeof(EnemySpawner), typeof(VisibilityChecker))]
public class Director : MonoBehaviour
{
    public event UnityAction<bool> PlayerVisibilityChanged;

    public static Director Instance { get; private set; }

    Player m_Player;
	public Player Player => m_Player;
    public Vector3? LastPlayerPos { get; private set; } = null;

    VisibilityChecker m_VisibilityChecker;
    public VisibilityChecker VisibilityChecker => m_VisibilityChecker;

    [SerializeField, Min(3)] float m_ChaseTime = 5;
    Timer m_ChaseTimer;

    bool m_PlayerVisible = false;
    public bool PlayerVisible
    {
        get => m_PlayerVisible;
        private set
        {
            if (m_PlayerVisible == value)
                return;
            DomainDebug.Log($"PlayerVisibilityChanged from {m_PlayerVisible} to {value}", DomainType.Director);
            m_PlayerVisible = value;
            PlayerVisibilityChanged?.Invoke(value);
        }
    }

    EnemySpawner m_Spawner;
    HashSet<Enemy> m_Enemies = null;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        m_Spawner = GetComponent<EnemySpawner>();
        m_VisibilityChecker = GetComponent<VisibilityChecker>();


        m_Enemies = new();
        m_ChaseTimer = new Timer(m_ChaseTime, false);

    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

	public void SetPlayer(Player p) => m_Player = p;
    void FindPlayer()
    {
        var player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            DomainDebug.LogError($"Player not found in the current level", DomainType.Director);
            return;
        }
        m_Player = player;
    }

    void OnEnable()
    {
        m_Spawner.Spawned += OnEnemySpawned;
        Person.Died += OnEnemyDied;
        LevelManager.Instance.LevelPostLoad += FindPlayer;
    }

    void OnDisable()
    {
        m_Spawner.Spawned -= OnEnemySpawned;
        Person.Died -= OnEnemyDied;
        LevelManager.Instance.LevelPostLoad -= FindPlayer;
    }

    void OnEnemySpawned(IReadOnlyCollection<Enemy> enemies)
    {
        m_Enemies.UnionWith(enemies);
        m_PlayerVisible = m_VisibilityChecker.IsPlayerVisible(m_Enemies);
    }

    void OnEnemyDied(Person p)
    {
        if (p is not Enemy e) return;
        DomainDebug.Log($"Enemy {e.name} removed", DomainType.Director);
        m_Enemies.Remove(e);
    }


    void CheckVisibility()
    {
        bool visibility = m_VisibilityChecker.IsPlayerVisible(m_Enemies);
        if (visibility)
        {
            PlayerVisible = true;
            LastPlayerPos = null;
            m_ChaseTimer.Deactivate();
            m_ChaseTimer.Reset();
        }
        else m_ChaseTimer.Activate();
    }

    void OnChased()
    {
        DomainDebug.Log($"Player chased", DomainType.Director);
        LastPlayerPos = m_Player.transform.position;
        PlayerVisible = false;
    }

    void FixedUpdate()
    {
        CheckVisibility();
        m_ChaseTimer.Update(Time.fixedDeltaTime);
    }

}
