/// <summary>
/// Represents an object that can receive damage.
/// </summary>
public interface IDamagable
{
    /// <summary>
    /// Applies damage to this object.
    /// </summary>
    /// <param name="damage">The amount of damage to apply. Must be non-negative.</param>
    /// <returns>
    /// <list type="bullet">
    /// <item><c>true</c> if damage was successfully applied (object was alive and damage > 0);</item>
    /// <item><c>false</c> otherwise (negative/zero damage or object already dead).</item>
    /// </list>
    /// </returns>
    bool TakeDamage(float damage);
}
