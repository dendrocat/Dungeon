using UnityEngine;
using UnityEngine.AI;
using TriInspector;

[DeclareBoxGroup("set", Title = "Settings")]
[DeclareFoldoutGroup("cmp", Title = "Components")]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Person
{
	[Group("set")]
	[SerializeField, Slider(5, 20)] float m_InWalkSpeed = 10;
	public float InWalkSpeed => m_InWalkSpeed;

	[Group("set")]
	[SerializeField, Slider(5, 20)] float m_InAttackSpeed = 20;
	public float InAttackSpeed => m_InAttackSpeed;

	[Group("cmp")]
	[SerializeField] Player m_Player;
    public Player Player => m_Player; 

    public NavMeshAgent NavAgent { get; private set; }
	
	[Group("cmp")]
	[SerializeField] EnemyAgent m_MLAgent;
	public EnemyAgent MLAgent => m_MLAgent;

	[Group("cmp")]
	[SerializeField] EnemyWeaponHandler m_WeaponHandler;
	public EnemyWeaponHandler WeaponHandler => m_WeaponHandler;

	void OnValidate() {
		if (m_InAttackSpeed < m_InWalkSpeed) m_InAttackSpeed = m_InWalkSpeed;
	}

    protected override void OnAwake()
    {
		NavAgent = GetComponent<NavMeshAgent>();
		NavAgent.speed = m_InWalkSpeed;

		m_WeaponHandler.SetTarget(m_Player.transform);
    }

    public void Init(Player player)
    {
		m_Player = player;
    }

    protected override void Die()
    {
        base.Die();
        NavAgent.enabled = false;
    }
}
