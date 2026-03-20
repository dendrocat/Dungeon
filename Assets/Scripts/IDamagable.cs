/// <summary>
/// Represents an object that can receive damage.
/// </summary>
public interface IDamagable
{
    /// <summary>
    /// Applies damage to this object.
    /// </summary>
    /// <param name="damage">The amount of damage to apply. Must be non-negative.</param>
    void TakeDamage(float damage);
}
