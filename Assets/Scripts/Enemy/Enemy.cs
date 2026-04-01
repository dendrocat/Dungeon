using UnityEngine;
using UnityEngine.AI;
using TriInspector;

[DeclareBoxGroup("set", Title = "Settings")]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Person
{
    [Group("set")]
    [SerializeField, Slider(5, 20)] float m_InWalkSpeed = 10;
    public float InWalkSpeed => m_InWalkSpeed;

    [Group("set")]
    [SerializeField, Slider(5, 20)] float m_InAttackSpeed = 20;
    public float InAttackSpeed => m_InAttackSpeed;

    public NavMeshAgent NavAgent { get; private set; }

    [Group("cmp")]
    [SerializeField] EnemyAgent m_MLAgent;
    public EnemyAgent MLAgent => m_MLAgent;

    [Group("cmp")]
    [SerializeField] EnemyWeaponHandler m_WeaponHandler;
    public EnemyWeaponHandler WeaponHandler => m_WeaponHandler;

    public new EnemyHealth Health => (p_Health as EnemyHealth);

    public IWaypointProvider WaypointProvider { get; private set; }

    void OnValidate()
    {
        if (m_InAttackSpeed < m_InWalkSpeed) m_InAttackSpeed = m_InWalkSpeed;
    }

    void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.speed = m_InWalkSpeed;

        m_WeaponHandler.SetTarget(Director.Instance.PlayerTransform);
    }

    public void SetWaypointProvider(IWaypointProvider provider)
    {
        WaypointProvider = provider;
    }

    protected override void Die()
    {
        base.Die();
        NavAgent.enabled = false;
    }

    void OnDisable()
    {
        DomainLogging.DomainDebug.Log($"{name} on disable", DomainLogging.DomainType.Person);
#if TRAIN || UNITY_EDITOR
        Die();
#endif
    }

    void OnDestroy()
    {
        DomainLogging.DomainDebug.Log($"{name} on destroy", DomainLogging.DomainType.Person);
    }
}
