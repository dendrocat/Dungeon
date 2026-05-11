using UnityEngine;
using UnityEngine.Serialization;
using TriInspector;

// [CreateAssetMenu(fileName = "WeaponStats", menuName = "Config/WeaponStats")]
[DeclareBoxGroup("set", Title = "Weapon Parameters")]
public abstract class WeaponStats : ScriptableObject
{
    [Required, AssetsOnly, PreviewObject, PropertyOrder(0)]
    [SerializeField] GameObject m_WeaponPrefab;
    public GameObject WeaponPrefab => m_WeaponPrefab;

    [Group("set"), PropertyOrder(2)]
    [LabelText("Type"), FormerlySerializedAs("Type")]
    [SerializeField] protected WeaponType p_Type;
    public WeaponType Type => p_Type;

    [Group("set"), PropertyOrder(3), Unit("HP")]
    [SerializeField, Min(1)] int m_Damage = 10;
    public int Damage => m_Damage;

	[Group("set"), PropertyOrder(4), Unit(UnitAttribute.Second)]
	[SerializeField, Min(0)] float m_UntilAttackTime = 0;
	public float UntilAttackTime => m_UntilAttackTime; 

    [Group("set"), PropertyOrder(10), Unit(UnitAttribute.Meter)]
    [SerializeField, Min(.1f)] float m_Distance = 1;
    public float Distance => m_Distance;

    [Group("set"), PropertyOrder(11)]
    [SerializeField, Min(1), Unit(UnitAttribute.Second)] float m_ReloadTime = 5;
    public float ReloadTime => m_ReloadTime;
}
