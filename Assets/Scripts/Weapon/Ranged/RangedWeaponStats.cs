using TriInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeaponStats", menuName = "Config/RangedWeaponStats")]
[DeclareBoxGroup("set/spread", Title = "Spread")]
[DeclareBoxGroup("ammo", Title = "Ammo Parameters")]
public class RangedWeaponStats : WeaponStats
{
    [Group("set"), PropertyOrder(10)]
    [SerializeField, Min(1), Unit("Shoots per minute")] int m_FireRate = 5;
    public int FireRate => m_FireRate;

    [Group("set"), PropertyOrder(11)]
    [SerializeField, Min(1), Unit("Bullets per shoot")] int m_BulletRate = 1;
    public int BulletRate => m_BulletRate;

    [Group("set/spread"), PropertyOrder(20)]
    [SerializeField, Slider(0.1f, 2f)] float x = 1f;

    [Group("set/spread"), PropertyOrder(21)]
    [SerializeField, Slider(0.1f, 2f)] float y = 1f;

    Vector2? m_MaxSpread = null;
    public Vector2 MaxSpread => m_MaxSpread.HasValue ? m_MaxSpread.Value : (m_MaxSpread = new Vector2(x, y)).Value;

    [Group("ammo")]
    [SerializeField, Required] Ammo m_AmmoPrefab;
    public Ammo AmmoPrefab => m_AmmoPrefab;

    [Group("ammo")]
    [SerializeField, Min(1)] int m_AmmoSpeed = 100;
    public int AmmoSpeed => m_AmmoSpeed;

    [Group("ammo")]
    [SerializeField, Min(1)] int m_MaxAmmoInTube = 12;
    public int MaxAmmoInTube => m_MaxAmmoInTube;

    [Group("ammo")]
    [SerializeField, Min(1)] int m_MaxAmmo = 12;
    public int MaxAmmo => m_MaxAmmo;
}
