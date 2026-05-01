using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[RequireComponent(typeof(PlayerWeaponHandler))]
public class WeaponSaveLoader : SaveLoader
{
    const string c_Key = "Weapons";

    [SerializeField] AssetLabelReference m_WeaponLabel;

    [System.Serializable]
    struct WeaponData
    {
        public int statsID;
        public int ammoInTube;
        public int ammo;
    }
    Dictionary<WeaponType, WeaponData> m_Data;

    PlayerWeaponHandler m_Hanlder;
    protected override void Awake()
    {
        m_Hanlder = GetComponent<PlayerWeaponHandler>();
    }
    void Start() { Load(); }

    void RestoreWeapons(Dictionary<WeaponType, WeaponStats> stats)
    {
        foreach (var (type, stat) in stats)
        {
            m_Hanlder.SwitchWeapon(stat);
            m_Hanlder.Weapons.GetValueOrDefault(type, null)?.SetAmmo(m_Data[type].ammoInTube, m_Data[type].ammo);
        }
    }

    protected override void Load()
    {
        if (!Repository.GetData(c_Key, out m_Data)) m_Data = new();
        if (m_Data.Count == 0) return;

        var stats = new Dictionary<WeaponType, WeaponStats>();

        var op = Addressables.LoadAssetsAsync<WeaponStats>(m_WeaponLabel, data =>
        {
            if (m_Data[data.Type].statsID == data.GetInstanceID())
                stats[data.Type] = data;
        });
        op.WaitForCompletion();
        op.Release();

        RestoreWeapons(stats);
    }

    protected override void Save()
    {
        foreach (var kv in m_Hanlder.Weapons)
        {
            m_Data[kv.Key] = new WeaponData
            {
                statsID = kv.Value.Stats.GetInstanceID(),
                ammoInTube = kv.Value.AmmoInTube,
                ammo = kv.Value.Ammo,
            };
        }
        m_Data[WeaponType.Melee] = new WeaponData
        {
            statsID = m_Hanlder.Melee.Stats.GetInstanceID()
        };
        Repository.SetData(c_Key, m_Data);
    }
}
