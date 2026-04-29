using UnityEngine;

[RequireComponent(typeof(PlayerAudioEmitter))]
public class AudioSaveLoader : SaveLoader
{
    const string c_Key = "AudioMultiplier";
    PlayerAudioEmitter m_Emitter;

    protected override void OnAwake()
    {
        m_Emitter = GetComponent<PlayerAudioEmitter>();
    }

    protected override void Load()
    {
        if (Repository.GetData(c_Key, out float multiplier))
            m_Emitter.VolumeMultiplier = multiplier;
    }

    protected override void Save()
    {
        Repository.SetData(c_Key, m_Emitter.VolumeMultiplier);
    }
}
