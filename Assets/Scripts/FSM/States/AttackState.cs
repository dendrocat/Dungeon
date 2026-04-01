using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AttackState : BaseState
{
    float m_AttackDistance;
    NavMeshAgent m_Agent;
    Transform m_Player;

    float baseStoppingDistance;

    float distanceOffset;

    bool m_PlayerVisible;

    protected override void OnEnter()
    {
        m_AttackDistance = p_Enemy.WeaponHandler.AttackDistance;
        m_Agent = p_Enemy.NavAgent;
        m_Player = Director.Instance.PlayerTransform;

        // baseStoppingDistance = m_Agent.stoppingDistance;
        // m_Agent.stoppingDistance = m_AttackDistance * 0.8f;
        m_Agent.speed = p_Enemy.InAttackSpeed;

        distanceOffset = m_AttackDistance / 3;

    }

    void SetDestination()
    {
        // Vector3 offset = (m_Player.position - p_Enemy.transform.position).normalized * m_Agent.stoppingDistance;
        Vector3 offset = Random.onUnitSphere * p_Enemy.WeaponHandler.AttackDistance / 2;
        offset.y = 0;
        if (NavMesh.SamplePosition(m_Player.position + offset,
                out var hit, 10f, NavMesh.AllAreas))
            m_Agent.SetDestination(hit.position);
        else m_Agent.SetDestination(m_Player.position);
    }

    protected override void OnUpdate(float dt)
    {
        if (!Director.Instance.PlayerVisible) { StateEnd(); return; }

        // if (m_Player == null || p_Enemy?.transform == null)
        // {
        //     DomainLogging.DomainDebug.LogWarning($"m_Player: {m_Player == null}, p_Enemy: {p_Enemy?.transform == null}");
        // }
        var dist = Vector3.Distance(m_Player.position, p_Enemy.transform.position);
        if (dist <= m_AttackDistance && Director.Instance.VisibilityChecker.IsPlayerVisibleFrom(p_Enemy))
        {
            p_Enemy.transform.LookAt(m_Player);
            p_Enemy.WeaponHandler.Attack();
        }
        else SetDestination();
    }

    protected override void OnExit()
    {
        m_Agent.speed = p_Enemy.InWalkSpeed;
        m_Agent.stoppingDistance = baseStoppingDistance;

        m_Agent = null;
        m_Player = null;
    }
}
