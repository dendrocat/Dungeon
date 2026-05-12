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

    protected void ResetStopTimer()
    {
        m_StopTimer.Reset(m_StopTime + Random.Range(-m_StopTime / 2, m_StopTime / 2));
    }

    protected virtual void SetDestination()
    {
        var n_Waypoint = p_Enemy.WaypointProvider.GetFreeWaypoint();
        var waypoint = n_Waypoint.Position;

        bool hitted = true;
        NavMeshHit hit;
        do
        {
            var nWaypoint = waypoint + Random.onUnitSphere * Waypoint.Radius;
            nWaypoint.y = 5;
            hitted = NavMesh.SamplePosition(nWaypoint, out hit, 10f, NavMesh.AllAreas);
        } while (!hitted);

        waypoint = hit.position;

        m_Waypoint?.Free();
        m_Waypoint = n_Waypoint;
        m_Waypoint.Occupy();

        NavMeshPath path = new();
        m_Agent.CalculatePath(waypoint, path);
        m_Agent.SetPath(path);

        ResetStopTimer();

        DomainDebug.Log($"{p_Enemy.name}: Moving to {m_Waypoint.Name}", DomainType.State);
        p_Enemy.Animator.Walk();
    }

    protected override void OnEnter()
    {
        m_Agent = p_Enemy.NavAgent;
        m_Agent.isStopped = false;

        m_StopTimer = new Timer(m_StopTime, false);
        m_StopTimer.TimerEnded += SetDestination;

        SetDestination();
    }


    protected override void OnUpdate(float dt)
    {
        // Debug.Log($"{m_Agent.hasPath} {m_StopTimer.IsActive}");
        if (!m_Agent.hasPath)
        {
            if (!m_StopTimer.IsActive)
            {
                m_StopTimer.Activate();
                p_Enemy.Animator.Idle();
            }
        }
        m_StopTimer.Update(dt);
        // if (Director.Instance.PlayerVisible) StateEnd();
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
