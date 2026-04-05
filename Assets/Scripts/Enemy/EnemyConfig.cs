using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/EnemyConfig")]
[DeclareBoxGroup("det", Title = "Detection Settings")]
public class EnemyConfig : PersonConfig<EnemyConfig.EnemyHealthConfig>
{
    [System.Serializable]
    public class EnemyHealthConfig : HealthConfig
    {
        [Unit("HP")]
        [SerializeField, Min(1f)] float m_HealValue = 1;
        public float HealValue => m_HealValue;

        [SerializeField, Min(1)] int m_HealCount = 1;
        public int HealCount => m_HealCount;
    }

    [System.Serializable]
    public class DetectionConfig
    {
        [SerializeField, Slider(0.1f, 1f)] float m_NightVisionLevel = 0.5f;
        public float NightVisionLevel => m_NightVisionLevel;

        [SerializeField, Slider(0f, 1f)] float m_AudioLevel = 0.5f;
        public float AudioLevel => m_AudioLevel;
    }
    [Group("det"), InlineProperty, HideLabel]
    [SerializeField] DetectionConfig m_Detection;
    public DetectionConfig Detection => m_Detection;
}
