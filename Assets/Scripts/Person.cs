using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TriInspector;

[DeclareFoldoutGroup("cmp", Title = "Components")]
public class Person : MonoBehaviour, IDamagable
{
    public static event UnityAction Died;

    [LabelText("Health")]
	[Group("cmp")]
    [SerializeField] protected Health p_Health;
	public Health Health => p_Health;

    IEnumerable<IActivatable> comps;

    void Awake()
    {
        comps = GetComponentsInChildren<IActivatable>();
        p_Health.Died += Die;
    }

	/// <inheritdoc/>
    public void TakeDamage(float damage)
    {
        p_Health.TakeDamage(damage);
    }

    protected virtual void Die()
    {
        foreach (var c in comps) c.Deactivate();
        Died?.Invoke();
    }
}
