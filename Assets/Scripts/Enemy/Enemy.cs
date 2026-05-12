using UnityEngine;
using UnityEngine.AI;
using TriInspector;

[DeclareBoxGroup("set", Title = "Settings")]
[RequireComponent(typeof(NavMeshAgent), typeof(EnemyAnimator))]
public class Enemy : Person<EnemyConfig, EnemyHealth, EnemyConfig.EnemyHealthConfig>
{
    public NavMeshAgent NavAgent { get; private set; }
    public EnemyAnimator Animator { get; private set; }

    [Group("cmp")]
    [SerializeField] EnemyAgent m_MLAgent;
    public EnemyAgent MLAgent => m_MLAgent;

    [Group("cmp")]
    [SerializeField] EnemyWeaponHandler m_WeaponHandler;
    public EnemyWeaponHandler WeaponHandler => m_WeaponHandler;

    public IWaypointProvider WaypointProvider { get; private set; }

    protected override void OnAwake()
    {
		Animator = GetComponent<EnemyAnimator>();

        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.speed = Config.Speed.BaseSpeed;

        m_WeaponHandler.SetTarget(Director.Instance.Player.transform);
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
        // Die();
#endif
    }

    void OnDestroy()
    {
        DomainLogging.DomainDebug.Log($"{name} on destroy", DomainLogging.DomainType.Person);
    }
}
