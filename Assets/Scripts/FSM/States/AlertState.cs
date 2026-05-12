using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AlertState : BaseState
{
    Vector3 GetPlayerPosFromRayPerception()
    {
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
        if (p_Enemy.MLAgent.AudioSensor.AudioOutput.AudioPosition.HasValue)
            return p_Enemy.MLAgent.AudioSensor.AudioOutput.AudioPosition.Value;
        return Vector3.positiveInfinity;
    }

    protected override void OnEnter()
    {
        // if (Director.Instance.PlayerVisible)
        // {
        //     StateEnd();
        //     return;
        // }
        Vector3 pos = GetPlayerPosFromRayPerception();
        if (pos.Equals(Vector3.positiveInfinity)) pos = GetAudioPos();
        if (pos.Equals(Vector3.positiveInfinity)) pos = Director.Instance.Player.transform.position;

        if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 10, NavMesh.AllAreas))
            pos = hit.position;
        p_Enemy.NavAgent.SetDestination(pos);
        p_Enemy.Animator.Walk();
    }

    // protected override void OnUpdate(float dt)
    // {
    //     if (p_Enemy.NavAgent.remainingDistance <= p_Enemy.NavAgent.stoppingDistance) StateEnd();
    // }
}
