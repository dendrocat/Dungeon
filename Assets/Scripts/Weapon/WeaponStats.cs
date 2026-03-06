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

    [Group("set"), PropertyOrder(6)]
    [SerializeField, Min(1)] int m_ReloadTime = 5;
    public int ReloadTime => m_ReloadTime;

}
