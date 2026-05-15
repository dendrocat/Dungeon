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
    bool m_IsAttacking = false;

    void OnAttacked()
    {
        m_IsAttacking = false;
    }

    protected override void OnEnter()
    {
        m_AttackDistance = p_Enemy.WeaponHandler.AttackDistance;
        m_Agent = p_Enemy.NavAgent;
        m_Player = Director.Instance.Player.transform;

        // baseStoppingDistance = m_Agent.stoppingDistance;
        // m_Agent.stoppingDistance = m_AttackDistance;
        m_Agent.speed = p_Enemy.Config.Speed.BaseSpeed * p_Enemy.Config.Speed.Multiplier;

        distanceOffset = m_AttackDistance / 3;

        p_Enemy.Animator.Attacked += OnAttacked;
    }

    void SetDestination(bool random = false)
    {
        if (m_IsAttacking) return;
        // DomainLogging.DomainDebug.Log($"{p_Enemy.name} set dest {m_Agent.remainingDistance} {m_Agent.velocity}");
        Vector3 GetOffset()
        {
            Vector3 offset;
            if (random)
                offset = Random.insideUnitSphere * m_AttackDistance / 3;
            else
                offset = (m_Player.position - p_Enemy.transform.position).normalized * m_AttackDistance * 0.2f;
            offset.y = 0;
            return offset;
        }

        int tries = 10;
        NavMeshHit hit;
        do
        {
            Vector3 nDest = m_Player.position + GetOffset();
            nDest.y = 5;
            NavMesh.SamplePosition(nDest, out hit, 10f, NavMesh.AllAreas);
        } while (!hit.hit && tries-- > 0);
        if (!hit.hit) return;

        NavMeshPath path = new();
        p_Enemy.NavAgent.CalculatePath(hit.position, path);
        p_Enemy.NavAgent.SetPath(path);

		p_Enemy.Animator.ResetAllTriggers();
        p_Enemy.Animator.Run();
    }

    protected override void OnUpdate(float dt)
    {
        // if (m_Player == null || p_Enemy?.transform == null)
        // {
        //     DomainLogging.DomainDebug.LogWarning($"m_Player: {m_Player == null}, p_Enemy: {p_Enemy?.transform == null}");
        // }

        float dist = Vector3.Distance(m_Player.position, p_Enemy.transform.position);

        bool needMove = dist > m_AttackDistance || dist < m_AttackDistance / 10;

        if (needMove)
        {
            bool needSetDest = !m_Agent.hasPath || Vector3.Distance(m_Agent.destination, m_Player.position) > m_AttackDistance;
            if (needSetDest) SetDestination();
            return;
        }
        bool localVisible = Director.Instance.VisibilityChecker.IsPlayerVisibleFrom(p_Enemy);

		// player not visible so set random destination
        if (!localVisible)
        {
            if (!m_Agent.hasPath) SetDestination(random: true);
            return;
        }

        // player already in attack range and visible
        if (m_Agent.hasPath) m_Agent.ResetPath();

        p_Enemy.transform.LookAt(m_Player);

        if (!p_Enemy.WeaponHandler.CanAttack())
        {
            p_Enemy.Animator.Idle();
            return;
        }
        if (m_IsAttacking) return;

        m_IsAttacking = true;

        p_Enemy.WeaponHandler.Attack();
        p_Enemy?.Animator?.Attack();

        // if (m_Agent != null && m_Agent.hasPath)
        //     DomainLogging.DomainDebug.Log($"{p_Enemy.name} check {m_Agent.remainingDistance} {m_Agent.velocity}");
    }

    protected override void OnExit()
    {
        p_Enemy.Animator.Attacked -= OnAttacked;

        m_Agent.speed = p_Enemy.Config.Speed.BaseSpeed;
        // m_Agent.stoppingDistance = baseStoppingDistance;

        m_Agent = null;
        m_Player = null;
    }
}
