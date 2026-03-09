using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Ammo : MonoBehaviour
{
    public event UnityAction Hitted;
    Rigidbody m_Rig;

    int m_Damage;
    int m_Speed;
    protected float p_Lifetime;

    [SerializeField] protected LayerMask HitMask;

    public virtual void Init(in RangedWeaponStats stats)
    {
        m_Rig = GetComponent<Rigidbody>();
        m_Damage = stats.Damage;
        m_Speed = stats.AmmoSpeed;

        p_Lifetime = stats.Distance / m_Speed;
    }

    public void Launch(Vector3 dir)
    {
        // Debug.Log($"{m_Rig.name} lauched");
        dir = dir.normalized;
        transform.forward = dir.normalized;
        m_Rig.linearVelocity = dir * m_Speed;
        Destroy(gameObject, p_Lifetime);
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
        // Debug.Log(other.gameObject.name);
        if (((1 << other.gameObject.layer) & HitMask.value) != 0) Attack(other.gameObject, m_Damage);
        Hitted?.Invoke();
        OnHit();
    }
}
