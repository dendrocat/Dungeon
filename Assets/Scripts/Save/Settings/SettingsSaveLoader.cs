using UnityEngine;

public abstract class SettingsSaveLoader : MonoBehaviour
{
    protected SettingsSO p_Defaults;
    public void SetDefaults(SettingsSO defaults)
        => p_Defaults = defaults;

    public virtual void Init() { }

    public abstract void Save();
    public abstract void Load();

    protected abstract void Apply();
    public void Restore() { OnRestore(); Apply(); }
    protected abstract void OnRestore();

    void OnDisable() { Apply(); }
}
