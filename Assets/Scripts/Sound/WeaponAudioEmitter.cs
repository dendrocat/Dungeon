using UnityEngine;
using TriInspector;

[DeclareFoldoutGroup("set", Title = "Settings", Expanded = true)]
[DeclareFoldoutGroup("cmp", Title = "Components")]
public class WeaponAudioEmitter : MonoBehaviour, IAudioEmitter
{
    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float FireVolume = 95f;

    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float MeleeVolume = 1f;

    [Group("set")]
    [SerializeField, Slider(0.01f, 100f)] float GrenadeVolume = 0.1f;

    public float GetAudioLevel()
    {
        return FireVolume;
    }

    void OnValidate()
    {
        if (FireVolume < MeleeVolume) FireVolume = 2 * MeleeVolume;
    }
}
