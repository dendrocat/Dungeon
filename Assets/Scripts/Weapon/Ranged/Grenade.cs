using UnityEngine;
using UnityEngine.Events;
using TriInspector;

public class Grenade : Ammo
{
    public event UnityAction Exploded;

    [PropertySpace(10)]
    [SerializeField, Min(10)] float m_ExplosionDamage = 20;
    [SerializeField, Slider(5, 20)] int m_ExplosionRadius = 5;
    [SerializeField, Slider(1, 10)] float m_ExplosionTime = 5;

    Timer m_ExplosionTimer;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_ExplosionRadius);
    }


    public override void Init(in RangedWeaponStats stats)
    {
        base.Init(stats);
        p_Lifetime = m_ExplosionTime + .1f;
        m_ExplosionTimer = new Timer(m_ExplosionTime);
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
            var damage = 1 - dist / m_ExplosionRadius;
            Attack(col.gameObject, m_ExplosionDamage * damage);
        }
    }

    void Update()
    {
        m_ExplosionTimer.Update(Time.deltaTime);
    }
}
