using System.Collections.Generic;
using UnityEngine;

public interface IWaypoinsStore
{
    public IReadOnlyList<Transform> Waypoints { get; }
}
