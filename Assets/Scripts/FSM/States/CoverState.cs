using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;
using TriInspector;

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
        if (hits.Length == 0) return Vector3.zero;

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

    protected override void OnEnter()
    {
        var cover = FindCover();
        Vector3 directFromPlayer = (cover - Director.Instance.Player.transform.position).normalized;
        Vector3 agentPos = cover + directFromPlayer * 5;

        if (NavMesh.SamplePosition(agentPos, out NavMeshHit hit, 10, NavMesh.AllAreas))
            agentPos = hit.position;

        p_Enemy.NavAgent.SetDestination(agentPos);
		p_Enemy.Animator.Walk();
    }

    protected override void OnUpdate(float dt)
    {
        if (p_Enemy.NavAgent.remainingDistance <= p_Enemy.NavAgent.stoppingDistance + 1f) Heal();
    }

    void Heal()
    {
		p_Enemy.Animator.Idle();
        if (p_Enemy.Health.Value / p_Enemy.Health.Max <= 0.5f)
            p_Enemy.Health.Heal();
        StateEnd();
    }
}
