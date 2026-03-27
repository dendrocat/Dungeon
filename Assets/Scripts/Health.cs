using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using TriInspector;

[DeclareBoxGroup("hset", Title = "Heath Settings")]
public abstract class Health : MonoBehaviour, IDamagable
{
    public event UnityAction Died;

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
    public virtual void TakeDamage(float damage)
    {
        if (p_Health <= 0) return;
        p_Health -= damage;
        if (p_Health <= 0) Died?.Invoke();
    }
}
