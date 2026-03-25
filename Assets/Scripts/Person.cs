using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TriInspector;
using DomainLogging;

[DeclareFoldoutGroup("cmp", Title = "Components")]
public abstract class Person : MonoBehaviour, IDamagable
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
        DomainDebug.Log($"{name} taken {damage}. Remainig health: {p_Health.Value}", DomainType.Person);
    }

    protected virtual void Die()
    {
        foreach (var c in comps) c.Deactivate();
        DomainDebug.Log($"{name} died", DomainType.Person);
        Died?.Invoke();
    }
}
