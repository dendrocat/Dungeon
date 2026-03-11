using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;
using TriInspector;

[System.Serializable]
public class PatrolState : BaseState
{
    [LabelWidth(100)]
	[FormerlySerializedAs("StopTime")]
    [SerializeField, Min(0.1f), Unit(UnitAttribute.Second)] float m_StopTime = 10;
    NavMeshAgent m_Agent;

    Timer m_StopTimer;
    IWaypoint m_Waypoint;

    void SetDestination()
    {
        m_StopTimer.Reset(m_StopTime + Random.Range(-m_StopTime / 2, m_StopTime / 2));

        m_Waypoint = WaypointsProvider.Instance.GetFreeWaypoint();
        var waypoint = m_Waypoint.Position;
        var nWaypoint = waypoint + Random.onUnitSphere * Waypoint.Radius;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(nWaypoint, out hit, 10f, NavMesh.AllAreas))
            waypoint = hit.position;
        else if (NavMesh.SamplePosition(waypoint, out hit, 10f, NavMesh.AllAreas))
            waypoint = hit.position;

        m_Agent.SetDestination(waypoint);
        m_Waypoint.Occupy();

        Debug.Log($"{p_Enemy.name}: Moving to {m_Waypoint.Name}");
    }

    protected override void OnEnter()
    {
        m_Agent = p_Enemy.NavAgent;
        m_Agent.isStopped = false;

        m_StopTimer = new Timer(m_StopTime);
        m_StopTimer.TimerEnded += SetDestination;

        SetDestination();
    }


    protected override void OnUpdate(float dt)
    {
        if (m_Agent.remainingDistance < m_Agent.stoppingDistance + 0.5f) m_StopTimer.Update(dt);
    }

    protected override void OnExit()
    {
        m_Agent.ResetPath();
        m_Agent = null;

        m_StopTimer = null;
    }

    protected override void OnContinue()
    {
        SetDestination();
    }
}
