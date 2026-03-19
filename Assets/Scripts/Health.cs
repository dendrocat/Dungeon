using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using TriInspector;

[DeclareBoxGroup("hset", Title = "Heath Settings")]
public class Health : MonoBehaviour, IDamagable
{
    public event UnityAction Died;

	[Group("hset")]
	[LabelText("Health"), PropertyOrder(0)]
    [ShowInInspector, DisableInEditMode]
    protected float p_Health;

	[Group("hset")]
	[LabelText("Max health"), FormerlySerializedAs("MaxHealth")]
    [SerializeField, Min(10)] 
	protected float p_MaxHealth = 100;

    protected virtual void Awake()
    {
        p_Health = p_MaxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (p_Health <= 0) return;
        p_Health -= damage;
        if (p_Health <= 0) Died?.Invoke();
    }
}
