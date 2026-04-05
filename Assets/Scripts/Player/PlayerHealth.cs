using UnityEngine;
using UnityEngine.Events;
using TriInspector;

[System.Serializable]
public class PlayerHealthConfig : HealthConfig
{
    [Unit(UnitAttribute.Second)]
    [SerializeField, Slider(2f, 10f)] float m_UntilRegenTime = 5f;
    public float UntilRegenTime => m_UntilRegenTime;

    [Unit("HP/s")]
    [SerializeField, Slider(1, 10f)] float m_RegenSpeed = 2f;
    public float RegenSpeed => m_RegenSpeed;
}

public class PlayerHealth : Health<PlayerHealthConfig>
{
    public event UnityAction<float> HealthChanged;

    Timer m_UntilRegen, m_Regen;

    float m_RegenSpeed;

    public override void Init(PlayerHealthConfig config)
    {
        base.Init(config);
        m_UntilRegen = new Timer(config.UntilRegenTime, false);
        m_UntilRegen.TimerEnded += Heal;

        m_Regen = new Timer(1, false);
        m_Regen.TimerEnded += Heal;

        m_RegenSpeed = config.RegenSpeed;
    }

    /// <inheritdoc/>
    public override bool TakeDamage(float damage)
    {
        if (!base.TakeDamage(damage)) return false;
        HealthChanged?.Invoke(p_Health);
        if (!m_UntilRegen.IsActive && !m_Regen.IsActive)
            m_UntilRegen.Activate();
        return true;
    }

    void Heal()
    {
        p_Health = Mathf.Min(p_Health + m_RegenSpeed, p_MaxHealth);
        HealthChanged?.Invoke(p_Health);
        if (p_Health == p_MaxHealth)
        {
            m_UntilRegen.Reset();
            return;
        }
        m_Regen.Activate();
        m_Regen.Reset();
    }

    void FixedUpdate()
    {
        m_UntilRegen.Update(Time.fixedDeltaTime);
        m_Regen.Update(Time.fixedDeltaTime);
    }
}
