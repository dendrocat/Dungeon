using System.Collections.Generic;
using UnityEngine;

public class SettingsSaveSystem : MonoBehaviour
{
	[SerializeField] Transform m_ParentLoaders;
    [SerializeField] SettingsSO m_Defaults;
    IReadOnlyCollection<SettingsSaveLoader> m_SaveLoaders;

    void Awake()
    {
        m_SaveLoaders = m_ParentLoaders.GetComponentsInChildren<SettingsSaveLoader>(true);
        foreach (var sl in m_SaveLoaders)
        {
            sl.SetDefaults(m_Defaults); 
			sl.Init();
        }
        Load();
    }

    public void Save()
    {
        foreach (var sl in m_SaveLoaders) sl.Save();
        SettingsRepository.Save();
    }

    void Load()
    {
        foreach (var sl in m_SaveLoaders) sl.Load();
    }

    public void Restore()
    {
        foreach (var sl in m_SaveLoaders) sl.Restore();
        Save();
    }
}
