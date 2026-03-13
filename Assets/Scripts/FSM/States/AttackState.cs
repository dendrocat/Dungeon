using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AttackState : BaseState
{
    float m_AttackDistance;
    NavMeshAgent m_Agent;
    Transform m_Player;

    float stoppingDistanceForAttack = 1f;
    float baseStoppingDistance;

    float distanceOffset;

    protected override void OnEnter()
    {
        m_AttackDistance = p_Enemy.WeaponHandler.AttackDistance;
        m_Agent = p_Enemy.NavAgent;
        m_Player = p_Enemy.Player.transform;

        baseStoppingDistance = m_Agent.stoppingDistance;
        m_Agent.stoppingDistance = m_AttackDistance;
		m_Agent.speed = p_Enemy.InAttackSpeed;

        distanceOffset = m_AttackDistance / 3;
    }

    void SetDestination()
    {
        NavMesh.SamplePosition(m_Player.position,
                out var hit, 10f, NavMesh.AllAreas);

        m_Agent.SetDestination(hit.position);
    }

    protected override void OnUpdate(float dt)
    {
        var dist = Vector3.Distance(m_Player.position, p_Enemy.transform.position);
        if (dist < (m_AttackDistance / 2 + distanceOffset) && m_Agent.hasPath) m_Agent.ResetPath();
        if (dist <= m_AttackDistance)
        {
			p_Enemy.transform.LookAt(m_Player);
            p_Enemy.WeaponHandler.Attack();
        }
        else SetDestination();
    }

    protected override void OnExit()
    {
        m_Agent.ResetPath();
		m_Agent.speed = p_Enemy.InWalkSpeed;
        m_Agent.stoppingDistance = baseStoppingDistance;

        m_Agent = null;
        m_Player = null;
    }
}
