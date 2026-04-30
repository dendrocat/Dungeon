using UnityEngine;

public abstract class SaveLoader : MonoBehaviour
{
    protected virtual void Awake() { }

    void OnEnable()
    {
        SaveSystem.Saving += Save;
        SaveSystem.Loaded += Load;
    }
    void OnDisable()
    {
        SaveSystem.Saving -= Save;
        SaveSystem.Loaded -= Load;
    }

    protected abstract void Save();

    protected abstract void Load();
}
