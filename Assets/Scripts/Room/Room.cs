using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DomainLogging;

[RequireComponent(typeof(SphereCollider))]
public class Room : MonoBehaviour, IActivatable, IWaypointProvider
{
    public static event UnityAction<Room> Activated;
    public event UnityAction PlayerExited;

    [SerializeField] Transform WaypointParent;
    IReadOnlyList<IWaypoint> m_Waypoints;

    [SerializeField] Transform SpawnParent;
    public IReadOnlyList<Transform> SpawnPoints { get; private set; }

    public bool IsActive { get; private set; }

    void Awake()
    {
        m_Waypoints = WaypointParent.GetComponentsInChildren<IWaypoint>();
        DomainDebug.Log($"{name}: found {m_Waypoints.Count} waypoints", DomainType.Room);
        SpawnPoints = SpawnParent.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        DomainDebug.Log($"{name}: found {SpawnPoints.Count} spawn points", DomainType.Room);
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsActive) return;
        DomainDebug.Log($"{other.name} entered in room", DomainType.Room);
        Activate();
    }

    void OnTriggerExit(Collider other)
    {
        PlayerExited?.Invoke();
    }

    public void Activate()
    {
        IsActive = true;
        Activated?.Invoke(this);
    }
    public void Deactivate()
    {
        IsActive = false;
    }

    public IWaypoint GetFreeWaypoint()
    {
        if (m_Waypoints == null || m_Waypoints.Count == 0) {
			DomainDebug.LogError("Waypoins not found", DomainType.Room);
			return null;
		}
        int index = 0;
        for (int attemp = 0; attemp < 10; ++attemp)
        {
            index = Random.Range(0, m_Waypoints.Count);
            if (m_Waypoints[index].IsBusy) continue;
            return m_Waypoints[index];
        }
        return m_Waypoints[0];
    }
}
