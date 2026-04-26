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
    public IReadOnlyDictionary<WeaponType, RangedWeapon> Weapons => m_Weapons;

    WeaponType m_CurrentWeapon = WeaponType.None;

    public MeleeWeapon Melee { get; private set; } = null;
    public RangedWeapon Weapon => (p_Weapon as RangedWeapon);
    public RangedWeapon Grenade => m_Weapons[WeaponType.Grenade];

    void Awake()
    {
        m_Weapons = new();
        foreach (var stats in m_WeaponsStats)
        {
            var weapon = new RangedWeapon(stats, transform);
            weapon.SetAmmo(stats.MaxAmmoInTube, stats.MaxAmmo);
            m_Weapons[stats.Type] = weapon;
        }

        var grenade = new RangedWeapon(m_GrenadeStats, transform);
        grenade.SetAmmo(m_GrenadeStats.MaxAmmoInTube, m_GrenadeStats.MaxAmmo - m_GrenadeStats.MaxAmmoInTube);
        m_Weapons[WeaponType.Grenade] = grenade;

        Melee = new MeleeWeapon(p_WeaponStats as MeleeWeaponStats, transform);
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
        if (Melee.Equiped) Melee.Unequip();

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
            int add = GetAdding(weapon.GetStats().MaxAmmo);
            weapon.AddAmmo(add);
            DomainDebug.Log($"{weapon.Stats.Type} added {add} bullets", DomainType.Weapon);
        }
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
        if (Melee.IsReloading) return;

        DomainDebug.Log($"Melee Attack", DomainType.Weapon);
        p_Weapon.Unequip();
        Melee.Equip();
        if (Melee.Attack()) RaiseAttacked(Melee);
    }

    public void SwitchWeapon(WeaponStats stats)
    {
        if (stats.Type == WeaponType.None) return;
        var type = stats.Type;

        IWeapon weapon = type switch
        {
            WeaponType.Melee => Melee,
            _ => m_Weapons.GetValueOrDefault(type, null),
        };
        if (stats.GetInstanceID() == weapon.Stats.GetInstanceID()) return;
		weapon.Unequip(true);

        IWeapon newWeapon = type switch
        {
            WeaponType.Melee => new MeleeWeapon(stats as MeleeWeaponStats, transform),
            _ => new RangedWeapon(stats as RangedWeaponStats, transform),
        };

        if (weapon is RangedWeapon weaponRanded)
        {
            (newWeapon as RangedWeapon).SetAmmo(weaponRanded.AmmoInTube, weaponRanded.Ammo);
        }

        if (type == WeaponType.Melee) Melee = newWeapon as MeleeWeapon;
        else m_Weapons[type] = newWeapon as RangedWeapon;

        if (type == m_CurrentWeapon) {
			p_Weapon = newWeapon;
			newWeapon.Equip();
		}
    }

    // public void SwitchWeapon(WeaponStats stats, bool _ = false)
    // {
    //     if (stats.Type == WeaponType.None) return;
    //     if (stats.Type == WeaponType.Melee)
    //     {
    //         if (Melee.Stats.GetInstanceID() == stats.GetInstanceID())
    //             return;
    //         Melee.Unequip(true);
    //         Melee = new MeleeWeapon(p_WeaponStats as MeleeWeaponStats, transform);
    //         return;
    //     }
    //     if (stats.Type == WeaponType.Grenade)
    //     {
    //         if (Grenade.Stats.GetInstanceID() == stats.GetInstanceID())
    //             return;
    //         var new_grenade = new RangedWeapon(stats as RangedWeaponStats, transform);
    //         new_grenade.SetAmmo(Grenade.AmmoInTube, Grenade.Ammo);
    //         Grenade.Unequip(true);
    //         Grenade = new_grenade;
    //         return;
    //     }
    //     if (m_Weapons[stats.Type].Stats.GetInstanceID() == stats.GetInstanceID())
    //         return;
    //     var new_weapon = new RangedWeapon(stats as RangedWeaponStats, transform);
    //     new_weapon.SetAmmo(0, m_Weapons[stats.Type].Ammo);
    //
    //     m_Weapons[stats.Type].Unequip(true);
    //     m_Weapons[stats.Type] = new_weapon;
    //
    //     if (stats.Type == m_CurrentWeapon)
    //         new_weapon.Equip();
    // }

    protected override void FixedUpdate()
    {
        if (p_Weapon.Equiped) p_Weapon.Update(Time.fixedDeltaTime);

        Grenade.Update(Time.fixedDeltaTime);
        Melee.Update(Time.fixedDeltaTime);
        if (Grenade.Equiped && (Grenade.Ammo <= 0 || Grenade.IsReloading))
        {
            Grenade.Unequip();
            p_Weapon.Equip();
        }
        if (Melee.Equiped && Melee.IsReloading)
        {
            Melee.Unequip();
            p_Weapon.Equip();
        }
    }
}
