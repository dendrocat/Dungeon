using UnityEngine;
using UnityEngine.AI;
using DomainLogging;

[System.Serializable]
public class AlertState : BaseState
{
    Vector3 GetPlayerPosFromRayPerception()
    {
        DomainDebug.Log($"{p_Enemy.MLAgent.RaySensor == null}", DomainType.State);
        DomainDebug.Log($"{p_Enemy.MLAgent.RaySensor?.RayPerceptionOutput == null}", DomainType.State);
        var rayOutputs = p_Enemy.MLAgent.RaySensor?.RayPerceptionOutput?.RayOutputs;
        if (rayOutputs != null)
            foreach (var rayOut in rayOutputs)
            {
                if (rayOut.HitTaggedObject)
                    return rayOut.HitGameObject.transform.position;
            }
        return Vector3.positiveInfinity;
    }

    Vector3 GetAudioPos()
    {
        return p_Enemy.MLAgent.AudioSensor.AudioOutput.AudioPosition;
    }

    protected override void OnEnter()
    {
        var pos = GetPlayerPosFromRayPerception();
        if (pos == Vector3.positiveInfinity) pos = GetAudioPos();

        if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 10, NavMesh.AllAreas))
            pos = hit.position;
        p_Enemy.NavAgent.SetDestination(pos);
    }

    protected override void OnUpdate(float dt)
    {
        if (p_Enemy.NavAgent.remainingDistance <= p_Enemy.NavAgent.stoppingDistance) StateEnd();
    }

    protected override void OnExit()
    {
        p_Enemy.NavAgent.ResetPath();
    }
}
