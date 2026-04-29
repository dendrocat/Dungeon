using UnityEngine;

public abstract class SaveLoader : MonoBehaviour
{
    void Awake()
    {
        SaveSystem.Saving += Save;
        SaveSystem.Loaded += Load;
		OnAwake();
    }
    protected virtual void OnAwake() { }

    protected abstract void Save();

    protected abstract void Load();
}
