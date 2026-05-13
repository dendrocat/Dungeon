using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TriInspector;
using DomainLogging;

public abstract class Person : MonoBehaviour
{
    public static event UnityAction<Person> Died;

    IEnumerable<IActivatable> comps;

    protected void Awake()
    {
        comps = GetComponentsInChildren<IActivatable>();
		InitHealth();
        OnAwake();
    }
	protected abstract void InitHealth();
    protected virtual void OnAwake() { }

    protected virtual void Die()
    {
        if (comps == null)
        {
            DomainDebug.LogError($"comps is NULL", DomainType.Person);
        }
        foreach (var c in comps) c.Deactivate();
        DomainDebug.Log($"{name} died", DomainType.Person);
        Died?.Invoke(this);
    }
}

[DeclareFoldoutGroup("cmp", Title = "Components")]
public abstract class Person<TConfig, THealth, THealthConfig> : Person, IDamagable
    where TConfig : PersonConfig<THealthConfig>
    where THealth : Health<THealthConfig>
    where THealthConfig : HealthConfig
{
    [SerializeField] TConfig m_Config;
    public TConfig Config => m_Config;

    [LabelText("Health")]
    [Group("cmp")]
    [SerializeField] protected THealth p_Health;
    public THealth Health => p_Health;

	protected sealed override void InitHealth() => p_Health.Init(m_Config.Health);

    /// <inheritdoc/>
    public bool TakeDamage(float damage)
    {
        // if (damage <= 0)
        // {
        // DomainDebug.LogError($"{name} recieved non-positive damage: {damage}", DomainType.Person);
        // }
        if (!p_Health.TakeDamage(damage)) return false;
        DomainDebug.Log($"{name} recieved {damage}. Remaining health: {p_Health.Value}", DomainType.Person);
        if (p_Health.Value <= 0) Die();
        return true;
    }

}
