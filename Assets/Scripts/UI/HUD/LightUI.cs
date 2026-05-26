using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LightUI : HUDComponent
{
    [SerializeField] RawImage m_Image;
    Player m_Player;

    [SerializeField] Color m_LightedColor;
    [SerializeField] Color m_UnlightedColor;

    bool m_IsLighted;

    Color GetColor() =>
         m_IsLighted ? m_LightedColor : m_UnlightedColor;

    public override void Init(Player player)
    {
        m_Player = player;
        m_IsLighted = m_Player.IsLighted;

        m_Image.color = GetColor();
    }

    Tween m_ColorTween;

    protected override void Update()
    {
        if (m_Player.IsLighted == m_IsLighted) return;
        m_IsLighted = m_Player.IsLighted;

        m_ColorTween?.Kill();

        m_ColorTween = m_Image.DOColor(GetColor(), 0.5f)
			.SetEase(Ease.OutCubic);
    }
}
