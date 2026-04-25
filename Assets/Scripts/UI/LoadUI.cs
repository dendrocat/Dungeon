using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadUI : MonoBehaviour, IActivatable
{
	[SerializeField] Slider m_Slider;
	[SerializeField] TextMeshProUGUI m_Value;

    public bool IsActive => gameObject.activeSelf;

    public void Activate() => gameObject.SetActive(true);

    public void Deactivate() => gameObject.SetActive(false);

    public void ShowProgress(float progress)
    {
        // Debug.Log(progress);
		m_Slider.value = progress;
		m_Value.text = (progress * 100).ToString("F0") + " %";
    }
}
