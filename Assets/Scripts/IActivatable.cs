public interface IActivatable
{
    public bool IsActive { get; }
    void Activate();
    void Deactivate();
}
