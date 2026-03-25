using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "MeleeWeaponStats", menuName = "Config/MeleeWeaponStats")]
public class MeleeWeaponStats : WeaponStats
{
    [Group("set"), PropertyOrder(4), Unit("HP")]
    [SerializeField, Min(1)] int m_BackDamage = 20;
    public int BackDamage => m_BackDamage;

    [Group("set"), PropertyOrder(100)]
    [SerializeField] LayerMask m_HitMask;
    public LayerMask HitMask => m_HitMask;

    void OnValidate()
    {
        p_Type = WeaponType.Melee;
    }
}
