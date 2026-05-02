using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "SettingsSO", menuName = "Scriptable Objects/SettingsSO")]
[DeclareBoxGroup("cont", Title = "Controls")]
[DeclareBoxGroup("qa", Title = "Quality")]
public class SettingsSO : ScriptableObject
{
    [System.Serializable]
    public class ControlsSettings
    {
        [Slider(0.1f, 2f)]
        public float Sensivity = 1;

        [HideInInspector]
        public string Rebinds = "";

        public ControlsSettings Clone() =>
            new ControlsSettings
            {
                Sensivity = Sensivity,
                Rebinds = Rebinds,
            };
    }
    [Group("cont"), InlineProperty, HideLabel]
    [SerializeField] ControlsSettings m_Controls;
    public ControlsSettings Controls => m_Controls.Clone();

    [System.Serializable]
    public class QualitySettings
    {
        public bool IsFullscreen = true;

        [HideInInspector] public int Resolution = -1;

        public bool VSync = true;

        [Slider(0, 2)]
        public int FPS = 0;

        [Slider(0, 2)]
        public int QualityLevel = -1;

        [Slider(-2, 2)]
        public float Brightness = 0;

        public QualitySettings Clone() =>
             new QualitySettings
             {
                 IsFullscreen = IsFullscreen,
                 Resolution = Resolution,
                 VSync = VSync,
                 FPS = FPS,
                 QualityLevel = QualityLevel,
                 Brightness = Brightness,
             };
    }
    [Group("qa"), InlineProperty, HideLabel]
    [SerializeField] QualitySettings m_Quality;
    public QualitySettings Quality => m_Quality.Clone();

    [System.Serializable]
    public class SoundSettings
    {
        [Slider(0f, 2f)]
        public float AllVolume = 1;

        public SoundSettings Clone() =>
            new SoundSettings
            {
                AllVolume = AllVolume,
            };
    }
    [Group("cont"), InlineProperty, HideLabel]
    [SerializeField] SoundSettings m_Sound;
    public SoundSettings Sound => m_Sound.Clone();
}
