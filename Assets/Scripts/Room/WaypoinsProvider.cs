using System.Collections.Generic;
using UnityEngine;

public class WaypointsProvider : MonoBehaviour
{
    public static WaypointsProvider Instance { get; private set; } = null;
    IReadOnlyList<IWaypoint> m_Waypoints;


    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void OnEnable()
    {
        Room.Activated += GetWaypoints;
    }
    void OnDisable()
    {
        Room.Activated -= GetWaypoints;
    }

    void GetWaypoints(IWaypoinsStore store)
    {
        m_Waypoints = store.Waypoints;
        Debug.Log($"Getted {m_Waypoints.Count} waypoints");
    }

    public IWaypoint GetFreeWaypoint()
    {
        if (m_Waypoints == null || m_Waypoints.Count == 0) throw new System.OperationCanceledException("Waypoints not found");
        int index = 0;
        for (int attemp = 0; attemp < 10; ++attemp)
        {
            index = Random.Range(0, m_Waypoints.Count);
            if (m_Waypoints[index].IsBusy) continue;
            return m_Waypoints[index];
        }
        return m_Waypoints[0];
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
