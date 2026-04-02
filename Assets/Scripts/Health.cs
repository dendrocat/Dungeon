using UnityEngine;
using UnityEngine.Serialization;
using TriInspector;

[DeclareBoxGroup("hset", Title = "Heath Settings")]
public abstract class Health : MonoBehaviour, IDamagable
{
    protected float p_Health;

    [Group("hset")]
    [LabelText("Health"), PropertyOrder(0)]
    [ShowInInspector, ReadOnly]
    public float Value => p_Health;

    [Group("hset")]
    [LabelText("Max health"), FormerlySerializedAs("MaxHealth")]
    [SerializeField, Min(10)]
    protected float p_MaxHealth = 100;
    public float Max => p_MaxHealth;

    protected virtual void Awake()
    {
        p_Health = p_MaxHealth;
    }

    /// <inheritdoc/>
    public virtual bool TakeDamage(float damage)
    {
        if (damage <= 0 || p_Health <= 0) return false;
        p_Health -= damage;
        return true;
    }
}
