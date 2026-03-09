using UnityEngine;
using TriInspector;

[RequireComponent(typeof(Grenade))]
public class GrenadeAudioEmitter : AmmoAudioEmitter
{
    [SerializeField, Slider(0.01f, 100f)] float m_ExplosionVolume = 100f;

    protected override void Awake()
    {
        base.Awake();
        var gr = p_Ammo as Grenade;
        gr.Exploded += OnExplode;
    }

    void OnExplode() => p_AudioLevel = m_ExplosionVolume;
}
