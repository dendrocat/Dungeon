using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;
using TriInspector;

[System.Serializable]
public class SearchState : PatrolState
{
    [LabelWidth(100)]
    [FormerlySerializedAs("SearchRange")]
    [Unit(UnitAttribute.Meter)]
    [SerializeField, MinMaxSlider(5, 40)] Vector2 m_SearchRange = new(5, 30);

    [LabelWidth(100)]
    [FormerlySerializedAs("RangeGrowTime")]
    [Unit(UnitAttribute.Second)]
    [SerializeField, Min(10)] float m_GrowTime = 10;
    Timer m_GrowTimer;

    float m_SearchRadius;

    protected override void OnEnter()
    {
        base.OnEnter();
        // Director.Instance.PlayerVisibilityChanged += OnVisibilityChanged;

        m_GrowTimer = new(m_GrowTime);
    }

    // private void OnVisibilityChanged(bool playerVisibility)
    // {
    //     if (playerVisibility) StateEnd();
    // }

    protected override void SetDestination()
    {
        Vector3 dest = Vector3.positiveInfinity;
        if (Director.Instance.LastPlayerPos.HasValue)
            dest = Director.Instance.LastPlayerPos.Value;
        else if (p_Enemy.MLAgent.AudioSensor.AudioOutput.AudioPosition.HasValue)
            dest = p_Enemy.MLAgent.AudioSensor.AudioOutput.AudioPosition.Value;
        else
            dest = Director.Instance.Player.transform.position;
        Debug.Log(dest);

        bool hitted = true;
        NavMeshHit hit;
        do
        {
            Vector3 nDest = dest + Random.onUnitSphere * m_SearchRadius;
            nDest.y = 5;
            hitted = NavMesh.SamplePosition(nDest, out hit, 10f, NavMesh.AllAreas);
        } while (!hitted);

        dest = hit.position;

        NavMeshPath path = new();
        p_Enemy.NavAgent.CalculatePath(dest, path);
        p_Enemy.NavAgent.SetPath(path);

        ResetStopTimer();

        p_Enemy.Animator.Walk();
    }

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        m_GrowTimer.Update(dt);

        m_SearchRadius = m_SearchRange.x + (m_SearchRange.y - m_SearchRange.x) * m_GrowTimer.Progress;
    }

    protected override void OnExit()
    {
        base.OnExit();
        m_GrowTimer = null;
        // Director.Instance.PlayerVisibilityChanged -= OnVisibilityChanged;
    }
}
