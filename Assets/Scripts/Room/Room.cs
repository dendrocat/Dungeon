using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Room : MonoBehaviour, IActivatable, IWaypoinsStore
{
    public static event UnityAction<Room> Activated;

    [SerializeField] Transform WaypointParent;
    public IReadOnlyList<Transform> Waypoints { get; private set; }

    public bool IsActive { get; private set; }

    void Awake()
    {
        Waypoints = WaypointParent.GetComponentsInChildren<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        Activate();
    }

    void OnTriggerExit(Collider other)
    {
        Deactivate();
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
}
