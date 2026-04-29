using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsSaveLoader : SettingsSaveLoader
{
    const string c_Rebinds = "rebinds";
    [SerializeField] InputActionAsset m_Asset;

    protected override void Load()
    {
        m_Asset.LoadBindingOverridesFromJson(SettingsRepository.GetSetting<string>(c_Rebinds));
    }

    protected override void Save()
    {
        SettingsRepository.SetSetting(c_Rebinds, m_Asset.SaveBindingOverridesAsJson());
    }
}
