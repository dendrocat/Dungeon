public class EnemyWeaponHandler : BaseWeaponHandler
{
    public float AttackDistance => p_WeaponStats.Distance;

    UnityEngine.Transform m_Target;

    void Awake()
    {
        if (p_WeaponStats is RangedWeaponStats stats)
        {
            p_Weapon = new RangedWeapon(stats, transform);
            (p_Weapon as RangedWeapon).SetAmmo(stats.MaxAmmoInTube, stats.MaxAmmo);
        }
        else p_Weapon = new MeleeWeapon(p_WeaponStats as MeleeWeaponStats, transform);
        p_Weapon.Equip();
    }

    public void SetTarget(UnityEngine.Transform target)
    {
        m_Target = target;
    }

    public override void Attack()
    {
        p_Weapon.Attack(m_Target.position);
    }
}
