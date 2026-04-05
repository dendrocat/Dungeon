using UnityEngine;
using TriInspector;

public abstract class HealthConfig
{
    [Unit("HP")]
    [SerializeField, Min(10)] float m_MaxHealth = 10;
    public float MaxHealth => m_MaxHealth;
}

[DeclareBoxGroup("hset", Title = "Heath Settings")]
public abstract class Health<THealthConfig> : MonoBehaviour
    where THealthConfig : HealthConfig
{
    protected float p_Health;

    [ShowInInspector, ReadOnly]
    public float Value => p_Health;

    protected float p_MaxHealth;
    public float Max => p_MaxHealth;

    public virtual void Init(THealthConfig config)
    {
        p_Health = p_MaxHealth = config.MaxHealth;
    }

    /// <inheritdoc/>
    public virtual bool TakeDamage(float damage)
    {
        if (damage <= 0 || p_Health <= 0) return false;
        p_Health -= damage;
        return true;
    }
}
