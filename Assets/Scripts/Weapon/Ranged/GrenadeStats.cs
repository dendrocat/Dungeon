using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "GrenadeStats", menuName = "Config/GrenadeStats")]
[DeclareBoxGroup("gset", Title = "Grenade Settings")]
public class GrenadeStats : AmmoStats 
{
	[Group("gset")]
    [SerializeField, Min(10)] float m_ExplosionDamage = 20;
	public float ExplosionDamage => m_ExplosionDamage;

	[Group("gset")]
    [SerializeField, Slider(5, 20)] int m_ExplosionRadius = 5;
	public int ExplosionRadius => m_ExplosionRadius;

	[Group("gset")]
    [SerializeField, Slider(1, 10)] float m_ExplosionTime = 5;
	public float ExplosionTime => m_ExplosionTime;

	[Group("gset")]
	[SerializeField, Slider(1, 10000)] float m_ExplosionForce = 20;
	public float ExplosionForce => m_ExplosionForce;
}
