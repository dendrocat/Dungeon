using UnityEngine;

public class MeleeWeapon : Weapon<WeaponStats>
{

    public MeleeWeapon(in WeaponStats stats, in Transform parent) : base(stats, parent)
    { }

    protected override void OnAttack(Vector3? target = null)
    {
        base.OnAttack();
		Collider[] cols = Physics.OverlapSphere(p_GObj.transform.position, p_Stats.Distance, p_Stats.HitMask);
		foreach (var col in cols)
			col.GetComponent<IDamagable>()?.TakeDamage(p_Stats.Damage);
    }
}
