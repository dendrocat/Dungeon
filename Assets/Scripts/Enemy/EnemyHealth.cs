using UnityEngine;
using TriInspector;

[DeclareBoxGroup("heset", Title = "Heal Settings")]
public class EnemyHealth : Health
{
    [Group("heset")]
    [Unit("HP")]
    [SerializeField, Min(1f)] float m_Heal;

    [Group("heset")]
    [SerializeField, Min(1)] int m_HealCount;

    public int RemainingHealCount { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        RemainingHealCount = m_HealCount;
    }

    public void Heal()
    {
        if (RemainingHealCount <= 0) return;
        p_Health = Mathf.Min(p_Health + m_Heal, p_MaxHealth);
        RemainingHealCount--;
    }
}
