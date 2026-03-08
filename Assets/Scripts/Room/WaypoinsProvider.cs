using System.Collections.Generic;
using UnityEngine;

public class WaypointsProvider : MonoBehaviour
{
    public static WaypointsProvider Instance { get; private set; } = null;
    IReadOnlyList<Transform> m_Waypoints;
    List<bool> m_BusyPoints;


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
        m_BusyPoints = new List<bool>(m_Waypoints.Count);
    }

    public Vector3 GetFreeWaypoint()
    {
        if (m_Waypoints == null || m_Waypoints.Count == 0) return Vector3.zero;
        for (int attemp = 0; attemp < 10; ++attemp)
        {
            int index = Random.Range(0, m_Waypoints.Count);
            if (m_BusyPoints[index]) continue;
            m_BusyPoints[index] = true;
            return m_Waypoints[index].position;
        }
        return m_Waypoints[0].position;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
