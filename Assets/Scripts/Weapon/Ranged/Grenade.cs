using UnityEngine;
using UnityEngine.Events;

public class Grenade : Ammo
{
    public event UnityAction Exploded;

    Timer m_ExplosionTimer;

    float m_ExplosionDamage = 20;
    int m_ExplosionRadius = 5;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_ExplosionRadius);
    }


    public override void Init(in RangedWeaponStats stats)
    {
        base.Init(stats);
        GrenadeStats s = stats.AmmoStats as GrenadeStats;
        m_ExplosionDamage = s.ExplosionDamage;
        m_ExplosionRadius = s.ExplosionRadius;

        m_ExplosionTimer = new Timer(s.ExplosionTime);
        m_ExplosionTimer.TimerEnded += Explode;
    }

    protected override void OnHit() { }

    void Explode()
    {
        Exploded?.Invoke();

        var colls = Physics.OverlapSphere(transform.position, m_ExplosionRadius, p_HitMask);

        foreach (var col in colls)
        {
            var dist = Vector3.Distance(transform.position, col.transform.position);
            var multip = 1 - dist / m_ExplosionRadius;
            Attack(col.gameObject, m_ExplosionDamage * multip);
        }
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        m_ExplosionTimer.Update(Time.fixedDeltaTime);
    }
}
