using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "AmmoStats", menuName = "Config/AmmoStats")]
[DeclareBoxGroup("set", Title = "Ammo Settings")]
public class AmmoStats : ScriptableObject
{
    [Group("set")]
    [SerializeField, Required] Ammo m_AmmoPrefab;
    public Ammo AmmoPrefab => m_AmmoPrefab;

    [Group("set")]
    [Unit(UnitAttribute.MeterPerSecond)]
    [SerializeField, Min(1)] int m_AmmoSpeed = 100;
    public int AmmoSpeed => m_AmmoSpeed;

    [Group("set"), PropertyOrder(100)]
    [SerializeField] LayerMask m_HitMask;
    public LayerMask HitMask => m_HitMask;

	[Group("set")]
	[SerializeField] GameObject m_OnHitPrefab = null;
	public GameObject OnHitPrefab => m_OnHitPrefab;
}
