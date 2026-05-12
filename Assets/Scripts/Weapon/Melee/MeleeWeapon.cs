using UnityEngine;

public class MeleeWeapon : Weapon<MeleeWeaponStats>
{
    public MeleeWeapon(in MeleeWeaponStats stats, in Transform parent) : base(stats, parent)
    { }

    bool IsBack(Transform target)
    {
        return Vector3.Dot(target.forward, (p_GObj.transform.position - target.position).normalized) < 0;
    }

    protected override void OnAttack(Vector3? target = null)
    {
		Debug.Log("Melee attack");
        Collider[] cols = Physics.OverlapSphere(p_GObj.transform.position, p_Stats.Distance, p_Stats.HitMask);

        int damage;
        foreach (var col in cols)
        {
            if (IsBack(col.transform)) damage = p_Stats.BackDamage;
            else damage = p_Stats.Damage;
            col.GetComponent<IDamagable>()?.TakeDamage(damage);
        }
        Reload();
    }
}
