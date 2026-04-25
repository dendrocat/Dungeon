using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
	List<ISaveLoader> m_SaveLoaders;

    void Start() { LoadData(); }

    void OnEnable()
    {
        Debug.Log("OnEnable");
    }
    void OnDisable()
    {
        Debug.Log("OnDisable");
    }

    void SaveData()
    {
        Debug.Log("SaveData: collecting data");
		// LevelManager.Instance.Save();
		// .... save weapons
		Repository.Save();
    }

    void LoadData()
    {
        Debug.Log("LoadData");
		Repository.Load();
		// LevelManager.Instance.Load();
    }
}
