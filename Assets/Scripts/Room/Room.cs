using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Room : MonoBehaviour, IActivatable, IProvider<IWaypoint>, IProvider<Transform>
{
    public static event UnityAction<IProvider<IWaypoint>> WapointsActivated;
    public static event UnityAction<IProvider<Transform>> SpawnActivated;

    [SerializeField] Transform WaypointParent;
    IReadOnlyList<IWaypoint> m_Waypoints;
    IReadOnlyList<IWaypoint> IProvider<IWaypoint>.Items => m_Waypoints;

    [SerializeField] Transform SpawnParent;
    IReadOnlyList<Transform> m_SpawnPoints;
    IReadOnlyList<Transform> IProvider<Transform>.Items => m_SpawnPoints;

    public bool IsActive { get; private set; }

    void Awake()
    {
        m_Waypoints = WaypointParent.GetComponentsInChildren<IWaypoint>();
        Debug.Log($"{name}: found {m_Waypoints.Count} waypoints");
        m_SpawnPoints = SpawnParent.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        Debug.Log($"{name}: found {m_SpawnPoints.Count} spawn points");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} entered in room");
        Activate();
    }

    void OnTriggerExit(Collider other)
    {
        Deactivate();
    }

    public void Activate()
    {
        IsActive = true;
        WapointsActivated?.Invoke(this);
        SpawnActivated?.Invoke(this);
    }
    public void Deactivate()
    {
        IsActive = false;
    }
}
