using UnityEngine;
using UnityEngine.Events;
using DomainLogging;

public class SaveSystem : MonoBehaviour
{
    public static event UnityAction Saving;
    public static event UnityAction Loaded;

    void Start()
    {
        LevelManager.LevelPreLoad += SaveData;
        LoadData();
    }

    void SaveData()
    {
        DomainDebug.Log("Collecting data", DomainType.Save);
        Saving?.Invoke();
        DomainDebug.Log("Data collected", DomainType.Save);
        Repository.Save();
    }

    void LoadData()
    {
        DomainDebug.Log("Loading data", DomainType.Save);
        Repository.Load();
        DomainDebug.Log("Data loaded", DomainType.Save);
        Loaded?.Invoke();
    }
}
