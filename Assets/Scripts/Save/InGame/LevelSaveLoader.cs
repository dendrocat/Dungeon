using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class LevelSaveLoader : SaveLoader
{
    const string c_Key = "Level";

    LevelManager m_LevelManager;
    protected override void OnAwake()
    {
        m_LevelManager = GetComponent<LevelManager>();
    }

    protected override void Load()
    {
        if (!Repository.GetData(c_Key, out long level))
            level = -1;
		Debug.Log($"Loaded level: {level}");
        m_LevelManager.LoadLevelByIndex((int)level);
    }

    protected override void Save()
    {
        Repository.SetData(c_Key, m_LevelManager.CurrentLevel + 1);
    }
}
