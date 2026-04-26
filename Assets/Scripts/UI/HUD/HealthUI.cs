using UnityEngine;
using TMPro;

public class HealthUI : HUDComponent
{
    PlayerHealth m_Health;

    [SerializeField] TextMeshProUGUI m_HP;

    public override void Init(Player player)
    {
        m_Health = player.Health;
    }

    void UpdateHP()
    {
        m_HP.text = $"{Mathf.RoundToInt(m_Health.Value)}";
    }

    protected override void Update()
    {
        UpdateHP();
    }
}
