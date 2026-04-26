using UnityEngine;
using TMPro;

public class AmmoUI : HUDComponent
{
    PlayerWeaponHandler m_WeaponHandler;

    [SerializeField] TextMeshProUGUI m_Ammo;
    [SerializeField] TextMeshProUGUI m_Grenade;

    public override void Init(Player player)
    {
        m_WeaponHandler = player.WeaponHandler;
    }

    void UpdateAmmo(TextMeshProUGUI text, RangedWeapon weapon)
    {
        text.text = $"{weapon.AmmoInTube} / {weapon.Ammo}";
    }

    protected override void Update()
    {
        UpdateAmmo(m_Ammo, m_WeaponHandler.Weapon);
        UpdateAmmo(m_Grenade, m_WeaponHandler.Grenade);
    }
}
