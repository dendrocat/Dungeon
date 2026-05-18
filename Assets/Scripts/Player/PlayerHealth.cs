using UnityEngine;
using UnityEngine.Events;
using TriInspector;

[System.Serializable]
public class PlayerHealthConfig : HealthConfig
{
    [Unit(UnitAttribute.Second)]
    [SerializeField, Slider(2f, 10f)] float m_UntilRegenTime = 5f;
    public float UntilRegenTime => m_UntilRegenTime;

    [Unit("HP/tk")]
    [SerializeField, Slider(1, 10f)] float m_RegenPerTick = 2f;
    public float RegenPerTick => m_RegenPerTick;

	[Unit(UnitAttribute.Second)]
	[SerializeField, Slider(0.1f, 2f)] float m_RegenInterval = 1f;
	public float RegenInterval => m_RegenInterval;
}

public class PlayerHealth : Health<PlayerHealthConfig>
{
    public event UnityAction HealthChanged;

    Timer m_UntilRegen, m_Regen;

    float m_RegenPerTick;

    public override void Init(PlayerHealthConfig config)
    {
        base.Init(config);
        m_UntilRegen = new Timer(config.UntilRegenTime, false);
        m_UntilRegen.TimerEnded += Heal;

        m_Regen = new Timer(config.RegenInterval, false);
        m_Regen.TimerEnded += Heal;

        m_RegenPerTick = config.RegenPerTick;
    }

    /// <inheritdoc/>
    public override bool TakeDamage(float damage)
    {
        if (!base.TakeDamage(damage)) return false;
        HealthChanged?.Invoke();
        if (!m_UntilRegen.IsActive && !m_Regen.IsActive)
            m_UntilRegen.Activate();
        return true;
    }

    void Heal()
    {
        p_Health = Mathf.Min(p_Health + m_RegenPerTick, p_MaxHealth);
        HealthChanged?.Invoke();
        if (p_Health == p_MaxHealth)
        {
            m_UntilRegen.Reset();
            return;
        }
        m_Regen.Reset();
        m_Regen.Activate();
    }

    void FixedUpdate()
    {
        m_UntilRegen.Update(Time.fixedDeltaTime);
        m_Regen.Update(Time.fixedDeltaTime);
    }
}
