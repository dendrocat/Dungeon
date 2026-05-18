using UnityEngine;
using TriInspector;

[DeclareBoxGroup("heset", Title = "Heal Settings")]
public class EnemyHealth : Health<EnemyConfig.EnemyHealthConfig>
{
    float m_HealValue;

    public int RemainingHealCount { get; private set; }

    public override void Init(EnemyConfig.EnemyHealthConfig config)
    {
        base.Init(config);
        m_HealValue = config.HealValue;
        RemainingHealCount = config.HealCount;
    }

    public void Heal()
    {
        if (RemainingHealCount <= 0) return;
		DomainLogging.DomainDebug.Log($"{transform.parent.name} healed", DomainLogging.DomainType.Person);
        p_Health = Mathf.Min(p_Health + m_HealValue, p_MaxHealth);
        RemainingHealCount--;
    }
}
