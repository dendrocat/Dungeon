using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "RangedWeaponStats", menuName = "Config/RangedWeaponStats")]
[DeclareBoxGroup("set/spread", Title = "Max Spread")]
public class RangedWeaponStats : WeaponStats
{
    [Group("set"), PropertyOrder(20)]
    [SerializeField, Min(1), Unit("RPM")] int m_FireRate = 5;
    public int FireRate => m_FireRate;

    [Group("set"), PropertyOrder(21)]
    [SerializeField, Min(1), Unit("BPS")] int m_BulletRate = 1;
    public int BulletRate => m_BulletRate;

    [Group("set"), PropertyOrder(22)]
    [SerializeField, Min(1)] int m_MaxAmmoInTube = 12;
    public int MaxAmmoInTube => m_MaxAmmoInTube;

    [Group("set"), PropertyOrder(23)]
    [SerializeField, Min(1)] int m_MaxAmmo = 12;
    public int MaxAmmo => m_MaxAmmo;

    [Group("set/spread"), PropertyOrder(30), Unit(UnitAttribute.Degree)]
    [SerializeField, Slider(0.1f, 30f)] float x = 1f;

    [Group("set/spread"), PropertyOrder(31), Unit(UnitAttribute.Degree)]
    [SerializeField, Slider(0.1f, 30f)] float y = 1f;

    Vector2? m_MaxSpread = null;
    public Vector2 MaxSpread => m_MaxSpread.HasValue ? m_MaxSpread.Value : (m_MaxSpread = new Vector2(x, y)).Value;

    [Required, InlineEditor]
    [SerializeField] AmmoStats m_AmmoStats;
    public AmmoStats AmmoStats => m_AmmoStats;
}
