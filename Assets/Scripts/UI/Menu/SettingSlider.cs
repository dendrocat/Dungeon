using UnityEngine;

public class SettingSlider : MonoBehaviour
{
    public event UnityEngine.Events.UnityAction<float> OnValueChanged;

    float m_Value;
    public float Value
    {
        get => m_Value; set
        {
            var clamped = Mathf.Clamp(value, m_Range.x, m_Range.y);
            if (clamped == m_Value) return;
            m_Value = value;
            OnValueChanged?.Invoke(m_Value);
        }
    }

    [SerializeField] UnityEngine.UI.Slider m_Slider;
    [SerializeField] TMPro.TextMeshProUGUI m_Text;

    [SerializeField] bool m_IsPercent = true;
    [SerializeField, TriInspector.MinMaxSlider(-10, 10)] Vector2 m_Range;

    void OnEnable()
    {
        OnValueChanged += UpdateSlider;
        m_Slider.onValueChanged.AddListener(OnSliderChanged);
        UpdateSlider(Value);
    }

    void OnDisable()
    {
        OnValueChanged -= UpdateSlider;
        m_Slider.onValueChanged.RemoveListener(OnSliderChanged);
    }

    float TransformValue(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        var val = Mathf.InverseLerp(oldMin, oldMax, value);
        // Debug.Log($"{value} {oldMin} {oldMax} {val}");
        var res = Mathf.Lerp(newMin, newMax, val);
        // Debug.Log($"{newMin} {newMax} {res}");
        return res;
    }

    void UpdateUI()
    {
        string str;
        if (m_IsPercent) str = $"{m_Slider.value * 100:0} %";
        else str = $"{m_Value:0.#}";
        m_Text.text = str;
    }

    void OnSliderChanged(float value)
    {
        Value = TransformValue(value, m_Slider.minValue, m_Slider.maxValue, m_Range.x, m_Range.y);
        UpdateUI();
    }

    void UpdateSlider(float value)
    {
        // Debug.Log("Updating slider");
        m_Slider.SetValueWithoutNotify(TransformValue(value, m_Range.x, m_Range.y, m_Slider.minValue, m_Slider.maxValue));
        UpdateUI();
    }
}
