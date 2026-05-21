using UnityEngine;
using TriInspector;

public class SoundBonus : Bonus
{
    [SerializeField, Slider(0.1f, 0.9f)] float m_VolumeMultiplier = 0.5f;

    protected override void OnInteract(Player player)
    {
        var emitter = player.GetComponentInChildren<PlayerAudioEmitter>();
		emitter.Volume = m_VolumeMultiplier;
    }
}
