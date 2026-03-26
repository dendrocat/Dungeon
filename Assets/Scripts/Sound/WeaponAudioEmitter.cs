using UnityEngine;
using TriInspector;

[DeclareFoldoutGroup("set", Title = "Settings", Expanded = true)]
[RequireComponent(typeof(BaseWeaponHandler))]
public class WeaponAudioEmitter : MonoBehaviour, IAudioEmitter
{
    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float m_MeleeVolume = 1f;

    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float m_FireVolume = 95f;

    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float m_GrenadeVolume = 0.1f;

    [Group("set")]
    [SerializeField, Slider(0.1f, 5f)] float m_AudioTime = 1f;

    Timer m_AudioTimer;
    WeaponType m_Type;

    void Awake()
    {
        GetComponent<BaseWeaponHandler>().Attacked += OnAttacked;
        m_AudioTimer = new Timer(m_AudioTime, false);
        m_AudioTimer.TimerEnded += () => m_Type = WeaponType.None;
    }

    void OnAttacked(WeaponStats stats)
    {
        m_Type = stats.Type;
        m_AudioTimer.Activate();
        m_AudioTimer.Reset();
    }

    public float GetAudioLevel() => m_Type switch
    {
        WeaponType.Melee => m_MeleeVolume,
        WeaponType.Fire => m_FireVolume,
        WeaponType.Grenade => m_GrenadeVolume,
        _ => 0
    };

    void OnValidate()
    {
        if (m_FireVolume < m_MeleeVolume) m_FireVolume = 2 * m_MeleeVolume;
    }

    void FixedUpdate()
    {
        m_AudioTimer.Update(Time.fixedDeltaTime);
    }
}
