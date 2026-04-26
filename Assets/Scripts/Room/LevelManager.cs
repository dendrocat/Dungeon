using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using DomainLogging;

public class LevelManager : MonoBehaviour
{
    static LevelManager Instance = null;

    public static event UnityAction LevelPreLoad;
    public static event UnityAction LevelPostLoad;
    public static event UnityAction EndedLevels;

    [SerializeField] List<AssetReferenceGameObject> m_Levels;

    public int CurrentLevel { get; private set; } = 0;

    GameObject m_LevelInstance;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        if (m_Levels.Count == 0)
        {
            DomainDebug.LogError($"Levels not setted", DomainType.Level);
            return;
        }
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void LoadLevel()
    {
        SceneLoader.Instance.LoadLevel(m_Levels[CurrentLevel], OnLevelLoaded);
    }

    void FindFinish()
    {
        var finish = FindFirstObjectByType<LevelFinish>();
        if (finish == null)
        {
            DomainDebug.LogError($"Finish not found in level {CurrentLevel + 1}: {m_LevelInstance.name}", DomainType.Level);
            return;
        }
        finish.LevelFinished += OnLevelFinished;
    }

    void OnLevelLoaded(GameObject level)
    {
        m_LevelInstance = Instantiate(level, Vector3.zero, Quaternion.identity);
        DomainDebug.Log($"Level {CurrentLevel + 1} instanced {m_LevelInstance.name}", DomainType.Level);
        FindFinish();

        LevelPostLoad?.Invoke();
    }

    void OnLevelFinished()
    {
        LevelPreLoad?.Invoke();

        Destroy(m_LevelInstance);
        CurrentLevel++;
        if (CurrentLevel >= m_Levels.Count) EndedLevels?.Invoke();
        else LoadLevel();
    }

    public void LoadLevelByIndex(int index)
    {
        if (index != -1)
            CurrentLevel = Mathf.Min(index, m_Levels.Count - 1);
        LoadLevel();
    }
}
