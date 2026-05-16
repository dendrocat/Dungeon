using UnityEngine;

public class Grenade : Ammo
{
    Timer m_ExplosionTimer;

	GrenadeStats GrenadeStats => p_Stats as GrenadeStats;

    void OnDrawGizmos()
    {
        if (GrenadeStats == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GrenadeStats.ExplosionRadius);
    }


    public override void Init(in RangedWeaponStats stats)
    {
        base.Init(stats);

        m_ExplosionTimer = new Timer(GrenadeStats.ExplosionTime);
        m_ExplosionTimer.TimerEnded += Explode;
    }

    protected override void OnHit()
    {
        if (!GrenadeStats.ExplodeOnHit) return;
        m_ExplosionTimer.Deactivate();
        Explode();
    }

    void Explode()
    {
		var stats = GrenadeStats;
        var colls = Physics.OverlapSphere(transform.position, stats.ExplosionRadius, p_Stats.HitMask);

        foreach (var col in colls)
        {
            var dist = Vector3.Distance(transform.position, col.transform.position);
            var multip = Mathf.Clamp01(1 - dist / stats.ExplosionRadius);
            Attack(col.gameObject, stats.ExplosionDamage * multip);
        }
        Spawn(stats.OnExplodePrefab);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        m_ExplosionTimer.Update(Time.fixedDeltaTime);
    }
}
