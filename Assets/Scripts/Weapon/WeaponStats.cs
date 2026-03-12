using TriInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Config/WeaponStats")]
[DeclareBoxGroup("set", Title = "Weapon Parameters")]
public class WeaponStats : ScriptableObject
{
    [Required, AssetsOnly, PreviewObject, PropertyOrder(0)]
    [SerializeField] GameObject m_WeaponPrefab;
    public GameObject WeaponPrefab => m_WeaponPrefab;

    [Group("set"), PropertyOrder(2)]
    [SerializeField, Min(1)] int m_Damage = 10;
    public int Damage => m_Damage;

    [Group("set"), PropertyOrder(3), Unit(UnitAttribute.Meter)]
    [SerializeField, Min(.1f)] float m_Distance = 1;
    public float Distance => m_Distance;

    [Group("set"), PropertyOrder(6)]
    [SerializeField, Min(1), Unit(UnitAttribute.Second)] int m_ReloadTime = 5;
    public int ReloadTime => m_ReloadTime;

	[Group("set"), PropertyOrder(100), PropertySpace(5)]
    [SerializeField] LayerMask m_HitMask;
	public LayerMask HitMask => m_HitMask;
}
