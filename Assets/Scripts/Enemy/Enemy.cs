using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Person 
{
	NavMeshAgent m_NavAgent;
	public NavMeshAgent NavAgent;

    protected override void Die()
    {
        base.Die();
		m_NavAgent.enabled = false;
    }
}
