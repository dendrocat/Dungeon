using UnityEngine;
using TMPro;

public class HealthUI : HUDComponent
{
    PlayerHealth m_Health;

    [SerializeField] TextMeshProUGUI m_HP;
	[SerializeField] UnityEngine.UI.Image m_Image;

    public override void Init(Player player)
    {
        m_Health = player.Health;
		m_Health.HealthChanged += UpdateHP;
    }

    protected override void Update() { }
    void UpdateHP()
    {
        m_HP.text = $"{Mathf.RoundToInt(m_Health.Value)}";
		m_Image.fillAmount = m_Health.Value / m_Health.Max;
    }
}
