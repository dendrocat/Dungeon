using UnityEngine;
using DomainLogging;

public class PlayerWeaponHandler : BaseWeaponHandler
{
    protected override string StatsLabel => "Melee Weapon Stats";
    [SerializeField] RangedWeaponStats[] m_Weapons;
    [SerializeField] RangedWeaponStats m_GrenadeStats;

    int[] m_Ammo, m_AmmoInTube;

    int m_Index = 0;

    MeleeWeapon m_Melee;
    public RangedWeapon Weapon => (p_Weapon as RangedWeapon);
    public RangedWeapon Grenade { get; private set; } = null;

    void Awake()
    {
        m_Ammo = new int[m_Weapons.Length];
        m_AmmoInTube = new int[m_Weapons.Length];
        for (int i = 0; i < m_Weapons.Length; ++i)
        {
            m_Ammo[i] = m_Weapons[i].MaxAmmo;
            m_AmmoInTube[i] = m_Weapons[i].MaxAmmoInTube;
        }

        Grenade = new RangedWeapon(m_GrenadeStats, transform);
        Grenade.SetAmmo(m_GrenadeStats.MaxAmmoInTube, m_GrenadeStats.MaxAmmo - m_GrenadeStats.MaxAmmoInTube);

        m_Melee = new MeleeWeapon(p_WeaponStats, transform);
    }

    public void ChangeWeapon(int index)
    {
        if (index - 1 == m_Index) return;
        if (Weapon != null)
        {
            m_Ammo[m_Index] = Weapon.Ammo;
            m_AmmoInTube[m_Index] = Weapon.AmmoInTube;
            Weapon.Unequip();
        }

        m_Index = index - 1;
        DomainDebug.Log($"Weapon index: {m_Index}, m_Weapons cnt : {m_Weapons.Length}", DomainType.Weapon);
        p_Weapon = new RangedWeapon(m_Weapons[m_Index], transform);
        (p_Weapon as RangedWeapon).SetAmmo(m_AmmoInTube[m_Index], m_Ammo[m_Index]);
        p_Weapon.Equip();
    }

    public override void Attack()
    {
        if (Grenade.Equiped) Grenade.Unequip(false);
        if (m_Melee.Equiped) m_Melee.Unequip(false);

        if (!p_Weapon.Equiped) p_Weapon.Equip();
        if (p_Weapon.Attack()) RaiseAttacked(m_Weapons[m_Index]);
    }

    public void AddAmmo()
    {
        Weapon.AddAmmo(Random.Range(5, 16));
        for (int i = 0; i < m_Weapons.Length; ++i)
        {
            if (m_Index == i) continue;
            m_Ammo[i] = Mathf.Min(m_Ammo[i] + Random.Range(5, 16), m_Weapons[i].MaxAmmo);
        }
    }

    public void ThrowGrenade()
    {
        if (Grenade.IsReloading) return;

        DomainDebug.Log($"Throw Grenade: {Grenade.ReloadProgress}", DomainType.Weapon);
        p_Weapon.Unequip(false);
        Grenade.Equip();
        if (Grenade.Attack()) RaiseAttacked(m_GrenadeStats);
    }

    public void MeleeAttack()
    {
        if (m_Melee.IsReloading) return;

        DomainDebug.Log($"Melee Attack", DomainType.Weapon);
        p_Weapon.Unequip(false);
        m_Melee.Equip();
        if (m_Melee.Attack()) RaiseAttacked(p_WeaponStats);
    }

    protected override void Update()
    {
        if (p_Weapon.Equiped) p_Weapon.OnUpdate();

        Grenade.OnUpdate();
        m_Melee.OnUpdate();
        if (Grenade.Equiped && (Grenade.Ammo <= 0 || Grenade.IsReloading))
        {
            Grenade.Unequip(false);
            p_Weapon.Equip();
        }
        if (m_Melee.Equiped && m_Melee.IsReloading)
        {
            m_Melee.Unequip(false);
            p_Weapon.Equip();
        }
    }
}
