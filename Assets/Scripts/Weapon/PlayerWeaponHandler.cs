using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour, IActivatable
{
    [SerializeField] RangedWeaponStats[] m_Weapons;
    [SerializeField] WeaponStats m_MeleeStats;
    [SerializeField] RangedWeaponStats m_GrenadeStats;

    int[] m_Ammo, m_AmmoInTube;

    int m_Index = 0;

    public bool IsActive => enabled;
    public RangedWeapon Weapon { get; private set; } = null;
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
    }

    void Start()
    {
        InputManager.Instance.WeaponNumed += ChangeWeapon;
        InputManager.Instance.Reloaded += Reload;
        InputManager.Instance.Throwed += ThrowGrenade;

        ChangeWeapon(1);
    }

    void ChangeWeapon(int index)
    {
        if (Weapon != null)
        {
            m_Ammo[m_Index] = Weapon.Ammo;
            m_AmmoInTube[m_Index] = Weapon.AmmoInTube;
            Weapon.Unequip();
        }

        m_Index = index - 1;
        Weapon = new RangedWeapon(m_Weapons[m_Index], transform);
        Weapon.SetAmmo(m_AmmoInTube[m_Index], m_Ammo[m_Index]);
        Weapon.Equip();
    }

    void Attack()
    {
        if (Grenade.Equiped) Grenade.Unequip(false);

        if (!Weapon.Equiped) Weapon.Equip();
        Weapon.Attack();
    }

    void Reload()
    {
        Weapon.Reload();
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

    void ThrowGrenade()
    {
        if (Grenade.IsReloading) return;

        Debug.Log("Throw Grenade");
        Weapon.Unequip(false);
        Grenade.Equip();
        Grenade.Attack();
    }

    void Update()
    {
        if (InputManager.Instance.Attack) Attack();
        if (Weapon.Equiped) Weapon.OnUpdate();

        Grenade.OnUpdate();
        if (Grenade.Equiped && (Grenade.Ammo <= 0 || Grenade.IsReloading))
        {
            Grenade.Unequip(false);
            Weapon.Equip();
        }
    }


    public void Activate() => enabled = true;
    public void Deactivate() => enabled = false;
}
