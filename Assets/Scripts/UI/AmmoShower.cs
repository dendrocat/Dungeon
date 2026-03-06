using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class AmmoShower : MonoBehaviour
{
    [SerializeField] WeaponHandler m_WeaponHandler;

	[Space(10)]
    [SerializeField] TextMeshProUGUI m_Ammo;
	
	[Space(10)]
    [SerializeField] Image m_GrenadeImage;
    [SerializeField] Image[] m_GrenadeShower;

    void UpdateAmmo()
    {
        m_Ammo.text = $"{m_WeaponHandler.Weapon.AmmoInTube} / {m_WeaponHandler.Weapon.Ammo}";
    }

    void UpdateGrenades()
    {
		var all_grenads = m_WeaponHandler.Grenade.Ammo + m_WeaponHandler.Grenade.AmmoInTube;
        for (int i = 0; i < m_GrenadeShower.Length; ++i)
            m_GrenadeShower[i].enabled = i < all_grenads; 
        m_GrenadeImage.fillAmount = 1 - m_WeaponHandler.Grenade.ReloadTimer / m_WeaponHandler.Grenade.ReloadTime;
    }

    void Update()
    {
        UpdateAmmo();
        UpdateGrenades();
    }
}
