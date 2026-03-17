using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamagable
{
    public event UnityAction<float> HealthChanged;
    public event UnityAction Died;

    [SerializeField, Min(10)] float m_MaxHealth = 100;

    [TriInspector.ShowInInspector, TriInspector.DisableInEditMode]
    float m_Health;

    void Awake()
    {
        m_Health = m_MaxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (m_Health <= 0) return;
        m_Health -= damage;
        HealthChanged?.Invoke(m_Health);
        if (m_Health <= 0) Died?.Invoke();
    }
}
