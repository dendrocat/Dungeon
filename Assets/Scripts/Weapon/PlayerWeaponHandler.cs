using UnityEngine;
using DomainLogging;
using System.Collections.Generic;

public class PlayerWeaponHandler : BaseWeaponHandler
{
    protected override string StatsLabel => "Melee Weapon Stats";

    [TriInspector.PropertyOrder(10)]
    [SerializeField] RangedWeaponStats[] m_WeaponsStats;
    [SerializeField] RangedWeaponStats m_GrenadeStats;

    Dictionary<WeaponType, RangedWeapon> m_Weapons;

    WeaponType m_CurrentWeapon = WeaponType.None;

    MeleeWeapon m_Melee;
    public RangedWeapon Weapon => (p_Weapon as RangedWeapon);
    public RangedWeapon Grenade { get; private set; } = null;

    void Awake()
    {
        m_Weapons = new();
        foreach (var stats in m_WeaponsStats)
        {
            var weapon = new RangedWeapon(stats, transform);
            weapon.SetAmmo(stats.MaxAmmoInTube, stats.MaxAmmo);
            m_Weapons[stats.Type] = weapon;
        }

        Grenade = new RangedWeapon(m_GrenadeStats, transform);
        Grenade.SetAmmo(m_GrenadeStats.MaxAmmoInTube, m_GrenadeStats.MaxAmmo - m_GrenadeStats.MaxAmmoInTube);

        m_Melee = new MeleeWeapon(p_WeaponStats as MeleeWeaponStats, transform);
    }

    public void ChangeWeapon(WeaponType type)
    {
        if (type == m_CurrentWeapon) return;
        if (Weapon != null)
            Weapon.Unequip();

        m_CurrentWeapon = type;
        p_Weapon = m_Weapons[m_CurrentWeapon];
        p_Weapon.Equip();
        DomainDebug.Log($"Switched to weapon: {type}, weapon cnt: {m_Weapons.Count}", DomainType.Weapon);
    }

    public override void Attack()
    {
        if (Grenade.Equiped) Grenade.Unequip();
        if (m_Melee.Equiped) m_Melee.Unequip();

        if (!p_Weapon.Equiped) p_Weapon.Equip();
        if (p_Weapon.Attack()) RaiseAttacked(p_Weapon);
    }

    int GetAdding(int maxAmmo)
    {
        return Random.Range(0, Mathf.Max(2, Mathf.CeilToInt(maxAmmo * 0.15f)));
    }

    public void AddAmmo()
    {
        foreach (var (_, weapon) in m_Weapons)
        {
            int add = GetAdding(weapon.MaxAmmo);
            weapon.AddAmmo(add);
            DomainDebug.Log($"{weapon.Type} added {add} bullets", DomainType.Weapon);
        }
        Grenade.AddAmmo(GetAdding(m_GrenadeStats.MaxAmmo));
    }

    public void ThrowGrenade()
    {
        if (Grenade.IsReloading) return;

        DomainDebug.Log($"Throw Grenade: {Grenade.ReloadProgress}", DomainType.Weapon);
        p_Weapon.Unequip();
        Grenade.Equip();
        if (Grenade.Attack()) RaiseAttacked(Grenade);
    }

    public void MeleeAttack()
    {
        if (m_Melee.IsReloading) return;

        DomainDebug.Log($"Melee Attack", DomainType.Weapon);
        p_Weapon.Unequip();
        m_Melee.Equip();
        if (m_Melee.Attack()) RaiseAttacked(m_Melee);
    }

    public void SwitchWeapon(WeaponStats stats)
    {
        if (stats.Type == WeaponType.Grenade || stats.Type == WeaponType.None) return;
        if (stats.Type == WeaponType.Melee)
        {
            m_Melee.Unequip(true);
            p_WeaponStats = stats;
            m_Melee = new MeleeWeapon(p_WeaponStats as MeleeWeaponStats, transform);
            return;
        }
        var new_weapon = new RangedWeapon(stats as RangedWeaponStats, transform);
        new_weapon.SetAmmo(0, m_Weapons[stats.Type].Ammo);

        m_Weapons[stats.Type].Unequip(true);
        m_Weapons[stats.Type] = new_weapon;

        if (stats.Type == m_CurrentWeapon)
            new_weapon.Equip();
    }

    protected override void FixedUpdate()
    {
        if (p_Weapon.Equiped) p_Weapon.Update(Time.fixedDeltaTime);

        Grenade.Update(Time.fixedDeltaTime);
        m_Melee.Update(Time.fixedDeltaTime);
        if (Grenade.Equiped && (Grenade.Ammo <= 0 || Grenade.IsReloading))
        {
            Grenade.Unequip();
            p_Weapon.Equip();
        }
        if (m_Melee.Equiped && m_Melee.IsReloading)
        {
            m_Melee.Unequip();
            p_Weapon.Equip();
        }
    }
}
