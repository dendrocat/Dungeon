using UnityEngine;
using UnityEngine.Events;
using TriInspector;

[DeclareBoxGroup("rset", Title = "Regen Settings")]
public class PlayerHealth : Health
{
    public event UnityAction<float> HealthChanged;

    [Group("rset")]
    [Unit(UnitAttribute.Second)]
    [SerializeField, Slider(2f, 10f)] float m_RegenTime = 5f;

    [Group("rset")]
    [Unit("HP/s")]
    [SerializeField, Slider(1, 10f)] float m_RegenSpeed = 2f;

    Timer m_UntilRegen, m_Regen;

    protected override void Awake()
    {
        base.Awake();
        m_UntilRegen = new Timer(m_RegenTime, false);
        m_UntilRegen.TimerEnded += Heal;

        m_Regen = new Timer(1, false);
        m_Regen.TimerEnded += Heal;
    }

    /// <inheritdoc/>
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        HealthChanged?.Invoke(p_Health);
        if (!m_UntilRegen.IsActive && !m_Regen.IsActive)
            m_UntilRegen.Activate();
    }

    void Heal()
    {
        p_Health += m_RegenSpeed;
        if (p_Health >= p_MaxHealth)
        {
            p_Health = p_MaxHealth;
            m_UntilRegen.Reset();
            return;
        }
        m_Regen.Activate();
        m_Regen.Reset();
    }

    void Update()
    {
        m_UntilRegen.Update(Time.deltaTime);
        m_Regen.Update(Time.deltaTime);
    }
}
