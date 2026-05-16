using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Ammo : MonoBehaviour
{
    Rigidbody m_Rig;

	int m_Damage;
	protected AmmoStats p_Stats;

    public virtual void Init(in RangedWeaponStats stats)
    {
        m_Rig = GetComponent<Rigidbody>();
		m_Damage = stats.Damage;
		p_Stats = stats.AmmoStats;
        Destroy(gameObject, 60);
    }

    public void Launch(Vector3 dir)
    {
        // DomainLogging.DomainDebug.Log($"{name} lauched", DomainLogging.DomainType.Weapon);
        transform.forward = dir.normalized;
        m_Rig.linearVelocity = dir * p_Stats.AmmoSpeed;
    }

    protected void Attack(GameObject target, float damage)
    {
        target.GetComponent<IDamagable>()?.TakeDamage(damage);
    }

    protected void Spawn(GameObject prefab)
    {
        if (prefab == null) return;
        Instantiate(prefab, transform.position, Quaternion.identity);
    }

    protected void Hit()
    {
		Spawn(p_Stats.OnHitPrefab);
		OnHit();
    }
    protected virtual void OnHit() { Destroy(gameObject); }

    void OnCollisionEnter(Collision other)
    {
        DomainLogging.DomainDebug.Log($"{name} hitted to {other.gameObject.name}", DomainLogging.DomainType.Weapon);
        if (((1 << other.gameObject.layer) & p_Stats.HitMask.value) != 0) Attack(other.gameObject, m_Damage);
        Hit();
    }
}
