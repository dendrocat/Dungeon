using UnityEngine;
using UnityEngine.Events;
using DomainLogging;

public static class SaveSystem
{
    public static event UnityAction Saving;
    public static event UnityAction Loaded;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void SubscribeSave()
    {
        LevelManager.LevelPreLoad += SaveData;
    }

    static void SaveData()
    {
        DomainDebug.Log("Collecting data", DomainType.Save);
        Saving?.Invoke();
        DomainDebug.Log("Data collected", DomainType.Save);
        Repository.Save();
    }

    public static void LoadData()
    {
        DomainDebug.Log("Loading data", DomainType.Save);
        Repository.Load();
        DomainDebug.Log("Data loaded", DomainType.Save);
        Loaded?.Invoke();
    }
}
