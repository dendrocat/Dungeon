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
        if (type == m_CurrentWeapon || type == WeaponType.None) return;

        void Equip()
        {
            m_CurrentWeapon = type;
            p_Weapon = m_Weapons[m_CurrentWeapon];
            p_Weapon.Equip();
            DomainDebug.Log($"Switched to weapon: {type}, weapon cnt: {m_Weapons.Count}", DomainType.Weapon);
        }

        if (p_Weapon != null)
        {
            if (p_Weapon.IsUnequiping) return;
            p_Weapon.Unequiped += Equip;
            p_Weapon.Unequip();
        }
        else Equip();
    }

    public override void Attack()
    {
        IWeapon equiped = null;
        if (Grenade.Equiped) equiped = Grenade;
        if (Melee.Equiped) equiped = Melee;

        if (equiped != null)
        {
            equiped.Unequiped += () =>
            {
                p_Weapon.Equip();
                if (p_Weapon.Attack()) RaiseAttacked(p_Weapon);
            };
            equiped.Unequip();
        }
        else
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

    void AttackSpecial(IWeapon weapon)
    {
        if (p_Weapon.IsUnequiping || !p_Weapon.Equiped || !weapon.CanAttack()) return;

        p_Weapon.Unequiped += () =>
        {
            weapon.Equip();
            if (weapon.Attack()) RaiseAttacked(weapon);
            DomainDebug.Log($"Attack special: {weapon.Stats.Type}", DomainType.Weapon);
        };
        p_Weapon.Unequip();
    }

    public void ThrowGrenade()
        => AttackSpecial(Grenade);

    public void MeleeAttack()
        => AttackSpecial(Melee);

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

        if (type == m_CurrentWeapon)
            weapon.Unequiped += () =>
            {
                p_Weapon = weapon;
                newWeapon.Equip();
            };
        weapon.Unequip(true);
    }

    void AutoUneqiup(IWeapon weapon)
    {
        if (!weapon.Equiped || weapon.IsUnequiping) return;

        bool shouldUnequip = true;
        if (!shouldUnequip && weapon is RangedWeapon ranged)
            shouldUnequip &= ranged.Ammo <= 0;

        if (!shouldUnequip) return;

        weapon.Unequiped += () => p_Weapon.Equip();
        weapon.Unequip();
    }

    protected override void FixedUpdate()
    {
        if (p_Weapon.Equiped) p_Weapon.Update(Time.fixedDeltaTime);

        Grenade.Update(Time.fixedDeltaTime);
        Melee.Update(Time.fixedDeltaTime);

        AutoUneqiup(Grenade);
        AutoUneqiup(Melee);
    }
}
