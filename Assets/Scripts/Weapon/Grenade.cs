using UnityEngine;
using TriInspector;

public class Grenade : Ammo
{
	[PropertySpace(10)]
    [SerializeField, Min(10)] float m_ExplosionDamage = 20;
    [SerializeField, Slider(5, 20)] int m_ExplosionRadius = 5;
    [SerializeField, Slider(1, 10)] float m_ExplosionTime = 5;

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		
		Gizmos.DrawWireSphere(transform.position, m_ExplosionRadius);
	}


    public override void Init(in RangedWeaponStats stats)
    {
        base.Init(stats);
        p_Lifetime = m_ExplosionTime + .1f;
    }

    protected override void OnHit() { }

    void Explode()
    {
        var colls = Physics.OverlapSphere(transform.position, m_ExplosionRadius, HitMask);

        foreach (var col in colls)
        {
            var r = Vector3.Distance(transform.position, col.transform.position);
            var m = 1 - r / m_ExplosionRadius;
            Attack(col.gameObject, m_ExplosionDamage * m);
        }

    }

    void Update()
    {
        if (m_ExplosionTime > 0)
            m_ExplosionTime -= Time.deltaTime;
        else Explode();
    }
}
