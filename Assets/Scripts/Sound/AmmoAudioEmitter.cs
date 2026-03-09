using UnityEngine;
using TriInspector;

[RequireComponent(typeof(Ammo))]
public class AmmoAudioEmitter : MonoBehaviour, IAudioEmitter
{
    [SerializeField, Slider(0.01f, 100f)] float m_HitVolume = 1f;

    protected float p_AudioLevel;

    protected Ammo p_Ammo;

    protected virtual void Awake()
    {
        p_Ammo = GetComponent<Ammo>();
        p_Ammo.Hitted += OnHit;
    }

    void OnHit() => p_AudioLevel = m_HitVolume;

    public float GetAudioLevel()
    {
        return p_AudioLevel;
    }
}
