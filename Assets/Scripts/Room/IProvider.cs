/// <summary>
/// Represents a provider that exposes a collection of items of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items provided by this interface.</typeparam>
public interface IProvider<T>
{
    /// <summary>
    /// An immutable list of items provided by this provider.
    /// </summary>
    public System.Collections.Generic.IReadOnlyList<T> Items { get; }
}
