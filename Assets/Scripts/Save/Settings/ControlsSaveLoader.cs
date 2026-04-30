using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
// using DomainLogging;

public class ControlsSaveLoader : SettingsSaveLoader
{
    public const string c_Key = "controls";
    [SerializeField] SettingSlider m_Sensitivity;
    [SerializeField] InputActionAsset m_Asset;

    SettingsSO.ControlsSettings m_Settings;

    public override void Load()
    {
        string json = SettingsRepository.GetSetting<string>(c_Key, "");
        // DomainDebug.Log(json, DomainType.UI);
        if (string.IsNullOrEmpty(json)) m_Settings = p_Defaults.Controls;
        else
            m_Settings = JsonConvert.DeserializeObject<SettingsSO.ControlsSettings>(json);
        Apply();
    }

    public override void Save()
    {
        var settings = m_Settings.Clone();
        settings.Sensivity = m_Sensitivity.Value;
        settings.Rebinds = m_Asset.SaveBindingOverridesAsJson();

        string json = JsonConvert.SerializeObject(settings);
        // DomainDebug.Log(json, DomainType.UI);
        SettingsRepository.SetSetting(c_Key, json);

        m_Settings = settings;
    }

    protected override void Apply()
    {
        m_Sensitivity.Value = m_Settings.Sensivity;

        m_Asset.RemoveAllBindingOverrides();
        m_Asset.LoadBindingOverridesFromJson(m_Settings.Rebinds);
    }

    protected override void OnRestore()
    {
        m_Settings = p_Defaults.Controls;
    }
}
