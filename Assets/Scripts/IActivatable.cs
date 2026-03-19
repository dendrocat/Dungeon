/// <summary>
/// Interface for objects that can be activated or deactivated.
/// </summary>
public interface IActivatable
{
    /// <summary>
    /// Current active state of the object.
    /// True = active, False = inactive.
    /// </summary>
    public bool IsActive { get; }

    /// <summary>
    /// Activates the object.
    /// </summary>
    void Activate();

    /// <summary>
    /// Deactivates the object.
    /// </summary>
    void Deactivate();
}
