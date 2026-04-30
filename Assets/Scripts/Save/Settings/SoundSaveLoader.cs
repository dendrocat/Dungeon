using UnityEngine;
using Newtonsoft.Json;
// using DomainLogging;

public class SoundSaveLoader : SettingsSaveLoader
{
    const string c_Key = "sound";

    [SerializeField] SettingSlider m_AllVolume;

    SettingsSO.SoundSettings m_Settings;

    public override void Init()
    {
        m_AllVolume.OnValueChanged += OnAllVolumeChanged;
    }

    void OnAllVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    public override void Load()
    {
        string json = SettingsRepository.GetSetting<string>(c_Key, "");
        // DomainDebug.Log(json, DomainType.UI);
        if (string.IsNullOrEmpty(json)) m_Settings = p_Defaults.Sound;
        else
            m_Settings = JsonConvert.DeserializeObject<SettingsSO.SoundSettings>(json);
        Apply();
    }

    public override void Save()
    {
        var settings = m_Settings.Clone();
        settings.AllVolume = m_AllVolume.Value;

        string json = JsonConvert.SerializeObject(settings);
        // DomainDebug.Log(json, DomainType.UI);
        SettingsRepository.SetSetting(c_Key, json);

        m_Settings = settings;
    }

    protected override void Apply()
    {
        m_AllVolume.Value = m_Settings.AllVolume;
    }

    protected override void OnRestore()
    {
        m_Settings = p_Defaults.Sound;
    }
}
