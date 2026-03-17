using System.Collections.Generic;
using UnityEngine;

public class WaypointsProvider : MonoBehaviour, IProvider<IWaypoint>
{
    public IReadOnlyList<IWaypoint> Items { get; private set; }

    void OnEnable()
    {
        Room.WapointsActivated += GetWaypoints;
    }
    void OnDisable()
    {
        Room.WapointsActivated -= GetWaypoints;
    }

    void GetWaypoints(IProvider<IWaypoint> store)
    {
        Items = store.Items;
        Debug.Log($"Getted {Items.Count} waypoints");
    }

    public IWaypoint GetFreeWaypoint()
    {
        if (Items == null || Items.Count == 0) throw new System.OperationCanceledException("Items not found");
        int index = 0;
        for (int attemp = 0; attemp < 10; ++attemp)
        {
            index = Random.Range(0, Items.Count);
            if (Items[index].IsBusy) continue;
            return Items[index];
        }
        return Items[0];
    }

}
