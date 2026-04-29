using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

using Dropdown = TMPro.TMP_Dropdown;

public class QualitySaveLoader : SettingsSaveLoader
{
    const int FPS = 60;
    const string c_Key = "Quality";

    [SerializeField] UnityEngine.UI.Slider m_Brightness;
    [SerializeField] Dropdown m_Quility;
    [SerializeField] Dropdown m_Resolution;
    [SerializeField] Dropdown m_ScreenMode;

    public struct QualitySettings
    {
        public float Brightness;
        public int QualityLevel;
        public int Resolution;
        public bool ScreenMode;

        public QualitySettings(
            float brightness = 1,
            int qualityLevel = -1,
            int resolution = -1,
            bool screenMode = true)
        {
            Brightness = brightness;
            QualityLevel = qualityLevel;
            Resolution = resolution;
            ScreenMode = screenMode;
        }
    }
    QualitySettings m_Settings;

    protected override void Save()
    {
        SettingsRepository.SetSetting(c_Key, JsonConvert.SerializeObject(m_Settings));
        Apply();
    }

    protected override void Load()
    {
        m_Settings = JsonConvert.DeserializeObject<QualitySettings>(SettingsRepository.GetSetting<string>(c_Key));
        Apply();
    }

    void Apply()
    {
        ApplyBrightness();
        ApplyQuality();
        ApplyScreen();
    }

    void ApplyBrightness()
    {
        m_Brightness.value = m_Settings.Brightness;
        Screen.brightness = m_Settings.Brightness;
    }

    void ApplyQuality()
    {
        m_Quility.value = m_Settings.QualityLevel;
        UnityEngine.QualitySettings.SetQualityLevel(m_Settings.QualityLevel);
    }

    void ApplyScreen()
    {
        m_Resolution.value = m_Settings.Resolution;
        var res = Screen.resolutions[m_Settings.Resolution];
        FullScreenMode mode = m_Settings.ScreenMode ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(res.width, res.height, mode, Screen.currentResolution.refreshRateRatio);
    }

    protected override void OnAwake()
    {
        SetResolutions();
    }

    void SetResolutions()
    {
        var resolutions = Screen.resolutions
            .Where(res => res.refreshRateRatio.value == FPS)
            .GroupBy(res => new { res.width, res.height })
            .Select(g => g.First())
            .OrderBy(res => res.width * res.height)
            .ToArray();
        // System.Text.StringBuilder b = new();
        // foreach (var res in resolutions) b.AppendLine($"{res.width}x{res.height}@{res.refreshRateRatio}");
        // Debug.Log(b.ToString());
        m_Resolution.ClearOptions();
        m_Resolution.AddOptions(resolutions.Select(res => new Dropdown.OptionData($"{res.width}x{res.height}")).ToList());

        m_Resolution.value = Array.FindIndex(resolutions,
                res => res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height
        );
    }
}
