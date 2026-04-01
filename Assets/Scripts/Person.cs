using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TriInspector;
using DomainLogging;

[DeclareFoldoutGroup("cmp", Title = "Components")]
public abstract class Person : MonoBehaviour, IDamagable
{
    public static event UnityAction<Person> Died;

    [LabelText("Health")]
    [Group("cmp")]
    [SerializeField] protected Health p_Health;
    public Health Health => p_Health;

    IEnumerable<IActivatable> comps;

    void Awake()
    {
        comps = GetComponentsInChildren<IActivatable>();
    }

    /// <inheritdoc/>
    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            // DomainDebug.LogError($"{name} recieved negative damage: {damage}", DomainType.Person);
            return;
        }
        p_Health.TakeDamage(damage);
        DomainDebug.Log($"{name} recieved {damage}. Remaining health: {p_Health.Value}", DomainType.Person);
        if (p_Health.Value <= 0) Die();
    }

    protected virtual void Die()
    {
        foreach (var c in comps) c.Deactivate();
        DomainDebug.Log($"{name} died", DomainType.Person);
        Died?.Invoke(this);
    }
}
