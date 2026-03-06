using UnityEngine;

public class MeleeWeapon : Weapon<WeaponStats>
{
    public MeleeWeapon(in WeaponStats stats, in Transform parent) : base(stats, parent)
    {
    }

    protected override void OnAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void	AfterReload()
    {
        throw new System.NotImplementedException();
    }
}
