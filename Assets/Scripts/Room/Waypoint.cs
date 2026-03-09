using UnityEngine;
using UnityEngine.AI;

public interface IWaypoint
{
    string Name { get; }

    bool IsBusy { get; }

    Vector3 Position { get; }

    void Occupy();
}

public class Waypoint : MonoBehaviour, IWaypoint
{
    [SerializeField] LayerMask m_CheckMask;

    public const int Radius = 10;

    public string Name => name;

    int m_NumOccupied = 0;

    public bool IsBusy => m_NumOccupied > 0;

    public Vector3 Position => transform.position;

    public void Occupy() => ++m_NumOccupied;

    void FixedUpdate()
    {
        if (!IsBusy) return;

        var cols = Physics.OverlapSphere(transform.position, Radius, m_CheckMask);
        foreach (var col in cols)
        {
            if (!col.TryGetComponent<NavMeshAgent>(out var agent)) continue;
            if (Vector3.Distance(agent.destination, transform.position) <= Radius) --m_NumOccupied;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, Radius);
    }
#endif
}
