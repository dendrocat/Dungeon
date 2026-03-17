using System.Collections.Generic;

public interface IProvider<T>
{
    public IReadOnlyList<T> Items { get; }
}
