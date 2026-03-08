using System;
using UnityEngine;
using UnityEngine.AI;
using TriInspector;

[Serializable]
public class PatrolState : BaseState
{
    [SerializeField, Slider(1, 10)] int m_StopTime;
    NavMeshAgent m_Agent;

    Timer m_StopTimer;

    void SetDestination()
    {
        Vector3 waypoint = WaypointsProvider.Instance.GetFreeWaypoint();
        if (NavMesh.SamplePosition(waypoint, out NavMeshHit hit, 1f, 1))
            waypoint = hit.position;
        m_Agent.SetDestination(waypoint);
        m_StopTimer.Reset(m_StopTime + UnityEngine.Random.Range(-m_StopTime / 2, m_StopTime / 2));
    }

    protected override void OnEnter()
    {
        m_StopTimer = new Timer(m_StopTime + UnityEngine.Random.Range(-m_StopTime / 2, m_StopTime / 2));
        m_StopTimer.TimerEnded += SetDestination;

        m_Agent = p_Enemy.NavAgent;
        SetDestination();
    }


    protected override void OnUpdate(float dt)
    {
        if (m_Agent.pathStatus == NavMeshPathStatus.PathComplete) m_StopTimer.Update(dt);
    }

    protected override void OnExit()
    {
        m_Agent.isStopped = true;
        m_Agent = null;
    }
}
