using UnityEngine;
using TriInspector;

[DeclareFoldoutGroup("set", Title = "Settings", Expanded = true)]
[DeclareFoldoutGroup("cmp", Title = "Components")]
public class MouseLook : MonoBehaviour
{
    [Slider(10, 100), Group("set")]
    [SerializeField] float m_Sensivity = 50;

    [Slider(70, 90), Group("set")]
    [SerializeField] int m_MaxAngle = 80;

    [Required, Group("cmp")]
    [SerializeField] Transform m_Cam;

    IInput m_Input;

    float xRot = 0;
    float m_SensivityMultiplier = 1;

    void LoadSettings()
    {
        var json = SettingsRepository.GetSetting(ControlsSaveLoader.c_Key, "");
        if (string.IsNullOrEmpty(json)) return;
        var settings = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsSO.ControlsSettings>(json);
        m_SensivityMultiplier = settings.Sensivity;
    }

    void Start()
    {
        m_Input = GetComponent<Player>().Input;
        LoadSettings();
		Debug.Log(QualitySettings.GetQualityLevel());
    }

    void FixedUpdate()
    {
        var dt = m_Input.MouseDelta * m_Sensivity * m_SensivityMultiplier * Time.fixedDeltaTime;

        xRot -= dt.y;
        xRot = Mathf.Clamp(xRot, -m_MaxAngle, m_MaxAngle);
        m_Cam.localRotation = Quaternion.Euler(xRot, 0, 0);

        transform.Rotate(Vector3.up * dt.x);
    }
}
