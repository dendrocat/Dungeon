using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;
using TriInspector;
using DomainLogging;

[System.Serializable]
public class PatrolState : BaseState
{
    [LabelWidth(100)]
    [FormerlySerializedAs("StopTime")]
    [Unit(UnitAttribute.Second)]
    [SerializeField, Slider(0.1f, 10f)] float m_StopTime = 10;
    NavMeshAgent m_Agent;

    Timer m_StopTimer;
    IWaypoint m_Waypoint;

    protected virtual void SetDestination()
    {
        var n_Waypoint = p_Enemy.WaypointProvider.GetFreeWaypoint();
        var waypoint = n_Waypoint.Position;
        var nWaypoint = waypoint + Random.onUnitSphere * Waypoint.Radius;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(nWaypoint, out hit, 10f, NavMesh.AllAreas))
            waypoint = hit.position;
        else if (NavMesh.SamplePosition(waypoint, out hit, 10f, NavMesh.AllAreas))
            waypoint = hit.position;

        m_Agent.SetDestination(waypoint);

        m_Waypoint?.Free();
        m_Waypoint = n_Waypoint;
        m_Waypoint.Occupy();

        m_StopTimer.Reset(m_StopTime + Random.Range(-m_StopTime / 2, m_StopTime / 2));
        m_StopTimer.Activate();

        DomainDebug.Log($"{p_Enemy.name}: Moving to {m_Waypoint.Name}", DomainType.State);
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
        if (m_Agent.remainingDistance < m_Agent.stoppingDistance + 2f)
            m_StopTimer.Update(dt);
        if (Director.Instance.PlayerVisible) StateEnd();
        // DomainDebug.Log($"{p_Enemy.name} stop timer isActive: {m_StopTimer.IsActive}, progress {m_StopTimer.Progress}", DomainType.State);
    }

    protected override void OnExit()
    {
        m_Waypoint?.Free();
        m_Waypoint = null;

        m_Agent = null;
        m_StopTimer = null;
    }

    protected override void OnContinue()
    {
        SetDestination();
    }
}
