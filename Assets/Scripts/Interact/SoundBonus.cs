using UnityEngine;
using TriInspector;

public class SoundBonus : Bonus
{
    [SerializeField] GameObject m_Prefab;
    protected override GameObject p_Prefab => m_Prefab;

    [SerializeField, Slider(0.1f, 0.9f)] float m_VolumeMultiplier = 0.5f;

    protected override void OnInteract(Player player)
    {
        var emitter = player.GetComponentInChildren<PlayerAudioEmitter>();
		emitter.SetVolumeMultiplier(m_VolumeMultiplier);
    }
}
