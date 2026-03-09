using System.Collections.Generic;

public interface IWaypoinsStore
{
    public IReadOnlyList<IWaypoint> Waypoints { get; }
}
