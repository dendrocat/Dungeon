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

    void SetDestination(bool random = false)
    {
        // DomainLogging.DomainDebug.Log($"{p_Enemy.name} set dest {m_Agent.remainingDistance} {m_Agent.velocity}");
        Vector3 offset;
        if (random)
            offset = Random.onUnitSphere * m_AttackDistance / 3;
        else
            offset = (m_Player.position - p_Enemy.transform.position).normalized * m_AttackDistance * 0.2f;
        offset.y = 0;
        if (NavMesh.SamplePosition(m_Player.position - offset,
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
        float dist = Vector3.Distance(m_Player.position, p_Enemy.transform.position);
        bool localVisible = Director.Instance.VisibilityChecker.IsPlayerVisibleFrom(p_Enemy);
        if (dist > m_AttackDistance)
            SetDestination();
        else
        {
            if (!localVisible)
            {
                p_Enemy.transform.LookAt(m_Player);
                localVisible = Director.Instance.VisibilityChecker.IsPlayerVisibleFrom(p_Enemy, false);
            }
            if (localVisible)
            {
                m_Agent.ResetPath();
                p_Enemy.transform.LookAt(m_Player);
                p_Enemy.WeaponHandler.Attack();
            }
            else if (!m_Agent.hasPath || m_Agent.velocity.magnitude < 0.1f)
                SetDestination(true);
        }
        // if (m_Agent != null && m_Agent.hasPath)
        //     DomainLogging.DomainDebug.Log($"{p_Enemy.name} check {m_Agent.remainingDistance} {m_Agent.velocity}");
    }

    protected override void OnExit()
    {
        m_Agent.speed = p_Enemy.InWalkSpeed;
        // m_Agent.stoppingDistance = baseStoppingDistance;

        m_Agent = null;
        m_Player = null;
    }
}
