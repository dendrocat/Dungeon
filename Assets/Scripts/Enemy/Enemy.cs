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

	[Group("set"), Slider(5, 20)] float m_InAttackSpeed = 20;

	[Group("cmp")]
	[SerializeField] Player m_Player;
    public Player Player => m_Player; 

    public NavMeshAgent NavAgent { get; private set; }
	
	[Group("cmp")]
	[SerializeField] EnemyAgent m_MLAgent;
	public EnemyAgent MLAgent => m_MLAgent;

	void OnValidate() {
		if (m_InAttackSpeed < m_InWalkSpeed) m_InAttackSpeed = m_InWalkSpeed;
	}

    protected override void OnAwake()
    {
		NavAgent = GetComponent<NavMeshAgent>();
		NavAgent.speed = m_InWalkSpeed;
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
