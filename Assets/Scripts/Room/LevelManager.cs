using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using DomainLogging;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } = null;

    public event UnityAction LevelPreLoad;
    public event UnityAction LevelPostLoad;
    public event UnityAction EndedLevels;

    [SerializeField] List<AssetReferenceGameObject> m_Levels;

    int m_CurrentLevel = 0;

    GameObject m_LevelInstance;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
    }
    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        if (m_Levels.Count == 0)
        {
            DomainDebug.LogError($"Levels not setted", DomainType.Level);
            return;
        }
        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        SceneLoader.Instance.LoadLevel(m_Levels[m_CurrentLevel], OnLevelLoaded);
    }

    void FindFinish()
    {
        var finish = FindFirstObjectByType<LevelFinish>();
        if (finish == null)
        {
            DomainDebug.LogError($"Finish not found in level {m_CurrentLevel + 1}: {m_LevelInstance.name}", DomainType.Level);
            return;
        }
        finish.LevelFinished += OnLevelFinished;
    }

    void OnLevelLoaded(GameObject level)
    {
        m_LevelInstance = Instantiate(level, Vector3.zero, Quaternion.identity);
        DomainDebug.Log($"Level {m_CurrentLevel + 1} instanced {m_LevelInstance.name}", DomainType.Level);
        FindFinish();

        LevelPostLoad?.Invoke();
    }

    void OnLevelFinished()
    {
        LevelPreLoad?.Invoke();
        Destroy(m_LevelInstance);
        m_CurrentLevel++;
        if (m_CurrentLevel >= m_Levels.Count) EndedLevels?.Invoke();
        else LoadNextLevel();
    }
}
