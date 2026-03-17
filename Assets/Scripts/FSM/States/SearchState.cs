using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;
using TriInspector;

[System.Serializable]
public class SearchState : PatrolState
{
    [LabelWidth(100)]
    [FormerlySerializedAs("SearchRadius")]
    [SerializeField, Slider(5, 20)] float m_SearchRadius = 5;

    protected override void SetDestination()
    {
        var dest = Director.Instance.LastPlayerPos.Value + Random.onUnitSphere * m_SearchRadius;

        if (NavMesh.SamplePosition(dest, out var hit, 10f, NavMesh.AllAreas))
            dest = hit.position;

        p_Enemy.NavAgent.SetDestination(dest);
    }
}
