using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AlertState : BaseState
{
    Vector3 GetPlayerPosFromRayPerception()
    {
		// TODO: null when Heuristic
		Debug.Log($"{p_Enemy.MLAgent.RaySensor == null}");
		Debug.Log($"{p_Enemy.MLAgent.RaySensor.RayPerceptionOutput == null}");
        var rayOutputs = p_Enemy.MLAgent.RaySensor.RayPerceptionOutput.RayOutputs;
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
