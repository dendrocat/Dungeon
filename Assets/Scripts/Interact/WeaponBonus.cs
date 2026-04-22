using UnityEngine;

public class WeaponBonus : Bonus
{
	[SerializeField] WeaponStats m_WeaponStats;
    protected override GameObject p_Prefab => m_WeaponStats.WeaponPrefab;

    protected override void OnInteract(Player player)
    {
		player.WeaponHandler.SwitchWeapon(m_WeaponStats);
    }
}
