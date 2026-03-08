using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour, IDamagable
{
    [SerializeField] protected Health p_Health;

    IEnumerable<IActivatable> comps;

    void Awake()
    {
        comps = GetComponentsInChildren<IActivatable>();
        p_Health.Died += Die;
    }

    public void TakeDamage(float damage)
    {
        p_Health.TakeDamage(damage);
    }

    protected virtual void Die()
    {
        foreach (var c in comps) c.Deactivate();
    }
}
