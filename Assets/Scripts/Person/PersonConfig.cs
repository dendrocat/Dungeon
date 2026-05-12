using UnityEngine;
using TriInspector;

[DeclareBoxGroup("sp", Title = "Speed Settings")]
[DeclareBoxGroup("hp", Title = "Health Settings")]
public abstract class PersonConfig<THealth> : ScriptableObject
    where THealth : HealthConfig
{
    [System.Serializable]
    public class SpeedConfig
    {
        [Unit(UnitAttribute.MeterPerSecond)]
        [SerializeField, Slider(2, 20)] float m_BaseSpeed = 10;
        public float BaseSpeed => m_BaseSpeed;

        [SerializeField, Slider(1f, 3f)] float m_Multiplier = 1.5f;
        public float Multiplier => m_Multiplier;

    }
    [Group("sp"), InlineProperty, HideLabel]
    [SerializeField] SpeedConfig m_Speed;
    public SpeedConfig Speed => m_Speed;

    [Group("hp"), InlineProperty, HideLabel]
    [SerializeField] THealth m_Health;
    public THealth Health => m_Health;
}
