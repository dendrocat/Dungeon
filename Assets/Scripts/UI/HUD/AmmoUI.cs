using UnityEngine;
using TMPro;

public class AmmoUI : HUDComponent
{
    PlayerWeaponHandler m_WeaponHandler;

    [SerializeField] TextMeshProUGUI m_Ammo;
    [SerializeField] TextMeshProUGUI m_Grenade;
    [SerializeField] UnityEngine.UI.Image m_GrenadeImage;

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
        if (m_WeaponHandler.Grenade.IsReloading)
            m_GrenadeImage.fillAmount = m_WeaponHandler.Grenade.ReloadProgress;
    }
}
