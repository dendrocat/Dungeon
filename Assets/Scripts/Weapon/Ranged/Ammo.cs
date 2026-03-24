using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Ammo : MonoBehaviour
{
    public event UnityAction Hitted;
    Rigidbody m_Rig;

    int m_Damage;
    int m_Speed;

    protected LayerMask p_HitMask;

    public virtual void Init(in RangedWeaponStats stats)
    {
        m_Rig = GetComponent<Rigidbody>();
        m_Damage = stats.Damage;
        m_Speed = stats.AmmoStats.AmmoSpeed;

        p_HitMask = stats.AmmoStats.HitMask;
		Destroy(gameObject, 1000);
    }

    public void Launch(Vector3 dir)
    {
        // DomainLogging.DomainDebug.Log($"{m_Rig.name} lauched", DomainLogging.DomainType.Weapon);
        dir = dir.normalized;
        transform.forward = dir.normalized;
        m_Rig.linearVelocity = dir * m_Speed;
    }

    protected void Attack(GameObject target, float damage)
    {
        target.GetComponent<IDamagable>()?.TakeDamage(damage);
    }

    protected virtual void OnHit()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        DomainLogging.DomainDebug.Log($"{name} hitted to {other.gameObject.name}", DomainLogging.DomainType.Weapon);
        if (((1 << other.gameObject.layer) & p_HitMask.value) != 0) Attack(other.gameObject, m_Damage);
        Hitted?.Invoke();
        OnHit();
    }
}
