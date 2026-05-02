using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using DomainLogging;

using Dropdown = TMPro.TMP_Dropdown;
using Toggle = UnityEngine.UI.Toggle;
using VolumeProfile = UnityEngine.Rendering.VolumeProfile;
using ColorAdjustments = UnityEngine.Rendering.Universal.ColorAdjustments;

public class QualitySaveLoader : SettingsSaveLoader
{
    const string c_Key = "Quality";

    [SerializeField] Dropdown m_ScreenMode;
    [SerializeField] Toggle m_VSync;
    [SerializeField] Dropdown m_FPS;
    readonly static int[] m_FPSValues = { -1, 30, 60 };

    [SerializeField] Dropdown m_Resolution;
    Resolution[] m_Resolutions;

    [SerializeField] Dropdown m_Quility;

    [SerializeField] VolumeProfile m_BrightnessVolumeProfile;
    ColorAdjustments m_ColorAdj = null;
    [SerializeField] SettingSlider m_Brightness;

    SettingsSO.QualitySettings m_Settings;

    public override void Init()
    {
        if (!m_BrightnessVolumeProfile.TryGet<ColorAdjustments>(out m_ColorAdj))
        {
            DomainDebug.LogError($"ColorAdjustments not found in {m_BrightnessVolumeProfile.name}", DomainType.UI);
            return;
        }

        InitResolutions();
        m_Brightness.OnValueChanged += OnBrightnessChanged;
		m_VSync.onValueChanged.AddListener(OnVSyncChanged);
    }

    void InitResolutions()
    {
        m_Resolutions = Screen.resolutions
            .GroupBy(res => new { res.width, res.height })
            .Select(g => g.First())
            .OrderBy(res => res.width * res.height)
            .ToArray();
        // System.Text.StringBuilder b = new();
        // foreach (var res in m_Resolutions) b.AppendLine($"{res.width}x{res.height}@{res.refreshRateRatio}");
        // DomainDebug.Log(b.ToString(), DomainType.UI);
        m_Resolution.ClearOptions();
        m_Resolution.AddOptions(m_Resolutions.Select(res => new Dropdown.OptionData($"{res.width}x{res.height}")).ToList());
    }

    void OnBrightnessChanged(float value)
    {
        m_ColorAdj.postExposure.value = value;
    }

	void OnVSyncChanged(bool value) {
		m_FPS.interactable = !value;
	}

    public override void Save()
    {
        var settings = m_Settings.Clone();
        settings.IsFullscreen = m_ScreenMode.value == 0;
        settings.Resolution = m_Resolution.value;
        settings.VSync = m_VSync.isOn;
        settings.FPS = m_FPS.value;
        settings.QualityLevel = m_Quility.value;
        settings.Brightness = m_Brightness.Value;

        string json = JsonConvert.SerializeObject(settings);
        // DomainDebug.Log(json, DomainType.UI);
        SettingsRepository.SetSetting(c_Key, json);

        m_Settings = settings;
        Apply();
    }

    public override void Load()
    {
        string json = SettingsRepository.GetSetting(c_Key, "");
        // DomainDebug.Log(json, DomainType.UI);
        if (string.IsNullOrEmpty(json)) m_Settings = p_Defaults.Quality;
        else
            m_Settings = JsonConvert.DeserializeObject<SettingsSO.QualitySettings>(json);
        Apply();
    }

    protected override void Apply()
    {
        ApplyScreen();
        ApplyFPS();
        ApplyQuality();
        m_Brightness.Value = m_Settings.Brightness;
    }

    void ApplyFPS()
    {
        m_VSync.isOn = m_Settings.VSync;
        m_FPS.interactable = !m_Settings.VSync;
		m_FPS.value = m_Settings.FPS;

        QualitySettings.vSyncCount = m_Settings.VSync ? 1 : 0;
        Application.targetFrameRate = m_FPSValues[m_Settings.FPS];
    }

    void ApplyQuality()
    {
        m_Quility.value = m_Settings.QualityLevel;
        QualitySettings.SetQualityLevel(m_Settings.QualityLevel);
    }

    void ApplyScreen()
    {
        int index = Mathf.Min(m_Settings.Resolution, m_Resolutions.Length - 1);
        if (index == -1) index = m_Resolutions.Length - 1;
        m_Resolution.value = index;
        var res = m_Resolutions[index];

        FullScreenMode mode = m_Settings.IsFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        m_ScreenMode.value = m_Settings.IsFullscreen ? 0 : 1;

        Screen.SetResolution(res.width, res.height, mode, Screen.currentResolution.refreshRateRatio);
    }

    protected override void OnRestore()
    {
        m_Settings = p_Defaults.Quality;
    }
}
