using UnityEngine;

public abstract class SettingsSaveLoader : MonoBehaviour
{
    void Awake()
    {
        SettingsSaveSystem.Saving += Save;
        SettingsSaveSystem.Loaded += Load;
        OnAwake();
    }
    protected virtual void OnAwake() { }

    protected abstract void Save();
    protected abstract void Load();
}
