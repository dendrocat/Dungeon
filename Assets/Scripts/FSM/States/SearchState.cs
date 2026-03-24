using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;
using TriInspector;

[System.Serializable]
public class SearchState : PatrolState
{
    [LabelWidth(100)]
    [FormerlySerializedAs("SearchRange")]
    [SerializeField, MinMaxSlider(5, 40)] Vector2 m_SearchRange = new(5, 30);

	[LabelWidth(100)]
	[FormerlySerializedAs("RangeGrowTime")]
	[SerializeField, Min(10)] float m_GrowTime = 10;
	Timer m_GrowTimer;

    float m_SearchRadius;

	[LabelWidth(100)]
    [FormerlySerializedAs("ChaseTime")]
    [SerializeField, Min(3)] float m_ChaseTime = 5;
    Timer m_ChaseTimer;

    protected override void OnEnter()
    {
        base.OnEnter();
        m_ChaseTimer = new Timer(m_ChaseTime, false);
        m_ChaseTimer.TimerEnded += StateEnd;
        Director.Instance.PlayerVisibilityChanged += OnVisibilityChanged;

		m_GrowTimer = new(m_GrowTime);
    }

    private void OnVisibilityChanged(bool playerVisibility)
    {
        if (playerVisibility) StateEnd();
    }

    protected override void SetDestination()
    {
        var dest = Director.Instance.LastPlayerPos.Value + Random.onUnitSphere * m_SearchRadius;

        if (NavMesh.SamplePosition(dest, out var hit, 10f, NavMesh.AllAreas))
            dest = hit.position;

        p_Enemy.NavAgent.SetDestination(dest);
    }

    protected override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        m_ChaseTimer.Update(dt);
		m_GrowTimer.Update(dt);

		m_SearchRadius = m_SearchRange.x + (m_SearchRange.y - m_SearchRange.x) * m_GrowTimer.Progress;
    }

    protected override void OnExit()
    {
        base.OnExit();
		m_ChaseTimer = null;
		m_GrowTimer = null;
		Director.Instance.PlayerVisibilityChanged -= OnVisibilityChanged;
    }
}
