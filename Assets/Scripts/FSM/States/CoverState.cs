using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;
using TriInspector;
using DomainLogging;

[System.Serializable]
public class CoverState : BaseState
{
    [LabelWidth(100)]
    [FormerlySerializedAs("DetectionRadius")]
    [Unit(UnitAttribute.Meter)]
    [SerializeField, Min(1f)] float m_DetectionRadius;

    [LabelWidth(100)]
    [FormerlySerializedAs("ObstacleLayer")]
    [SerializeField] LayerMask m_ObstacleLayer;

    float GetScore(in Transform hit)
    {
        float dCoverEnemy, dCoverPlayer;
        dCoverEnemy = (hit.position - p_Enemy.transform.position).sqrMagnitude;
        dCoverPlayer = (hit.position - Director.Instance.Player.transform.position).sqrMagnitude;
        return dCoverEnemy - dCoverPlayer;
    }


    Vector3 FindCover()
    {
        Collider[] hits = Physics.OverlapSphere(p_Enemy.transform.position, m_DetectionRadius, m_ObstacleLayer);
        DomainDebug.Log($"{p_Enemy.name} founded {hits.Length} covers", DomainType.State);
        if (hits.Length == 0) return Vector3.positiveInfinity;

        float minScore = float.MaxValue;
        Vector3 obstaclePosition = hits[0].transform.position;
        foreach (var hit in hits)
        {
            float score = GetScore(hit.transform);
            if (score < minScore)
            {
                minScore = score;
                obstaclePosition = hit.transform.position;
            }
        }
        return obstaclePosition;
    }

    void SetDestination()
    {
        var cover = FindCover();
        if (cover.Equals(Vector3.positiveInfinity)) { StateEnd(); return; }
        Vector3 directFromPlayer = (cover - Director.Instance.Player.transform.position).normalized;
        Vector3 agentPos = cover + directFromPlayer * 5;

        if (!NavMesh.SamplePosition(agentPos, out NavMeshHit hit, 10, NavMesh.AllAreas)) { StateEnd(); return; }

        NavMeshPath path = new();
        p_Enemy.NavAgent.CalculatePath(hit.position, path);
        p_Enemy.NavAgent.SetPath(path);
    }

    protected override void OnEnter()
    {
        // if (p_Enemy.Health.RemainingHealCount <= 0) { StateEnd(); return; }
        p_Enemy.NavAgent.speed = p_Enemy.Config.Speed.BaseSpeed * p_Enemy.Config.Speed.Multiplier;
        SetDestination();

        p_Enemy.Animator.Run();
    }

    protected override void OnUpdate(float dt)
    {
        if (p_Enemy.NavAgent.remainingDistance <= p_Enemy.NavAgent.stoppingDistance + 1f) Heal();
    }

    void Heal()
    {
        p_Enemy.Animator.Idle();
        p_Enemy.Health.Heal();
        StateEnd();
    }

    protected override void OnExit()
    {
        p_Enemy.NavAgent.speed = p_Enemy.Config.Speed.BaseSpeed;
    }
}
