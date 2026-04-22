using UnityEngine;
using TriInspector;

[DeclareFoldoutGroup("set", Title = "Settings", Expanded = true)]
[DeclareFoldoutGroup("cmp", Title = "Components")]
public class PlayerAudioEmitter : MonoBehaviour, IAudioEmitter
{
    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float CrouchVolume = 0.1f;

    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float WalkVolume = 20f;

    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float RunVolume = 70f;

    [Group("cmp")]
    [SerializeField] PlayerMover m_Mover;

    float m_Multiplier = 1;
    public void SetVolumeMultiplier(float multiplier)
    {
        m_Multiplier = multiplier;
    }

    float GetAudio()
    {
        var speed = m_Mover.Speed;
        if (speed < m_Mover.BaseSpeed) return CrouchVolume;
        if (speed == m_Mover.BaseSpeed) return WalkVolume;
        return RunVolume;
    }

    public float GetAudioLevel()
    {
        return GetAudio() * m_Multiplier;
    }

    void OnValidate()
    {
        if (WalkVolume < CrouchVolume) WalkVolume = CrouchVolume;
        if (RunVolume < WalkVolume) RunVolume = WalkVolume;
    }
}
