using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsSaveSystem : MonoBehaviour
{
	public static event UnityAction Saving;
	public static event UnityAction Loaded;

	[SerializeField] Transform m_ParentSettings;
	IReadOnlyCollection<SettingsSaveLoader> m_SaveLoaders;

    void Start()
    {
		m_SaveLoaders = m_ParentSettings.GetComponentsInChildren<SettingsSaveLoader>();
        Load();
    }

    public void Save()
    {
		Saving?.Invoke();
        SettingsRepository.Save();
    }

    public void Load()
    {
		Loaded?.Invoke();
        SettingsRepository.Load();
    }
}
