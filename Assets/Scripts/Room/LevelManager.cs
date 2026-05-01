using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DomainLogging;

public class LevelManager : MonoBehaviour
{
    public static event UnityAction LevelPreLoad;
    public static event UnityAction EndedLevels;

    [TriInspector.Scene]
    [SerializeField] List<string> m_Levels;

    public int CurrentLevel { get; private set; } = 0;
    public int NextLevel => Mathf.Min(CurrentLevel + 1, m_Levels.Count - 1);

    void Awake()
    {
        if (m_Levels.Count == 0)
        {
            DomainDebug.LogError($"Levels not setted", DomainType.Level);
            return;
        }
    }

    void OnEnable()
    {
        SceneLoader.Instance.LevelLoaded += OnLevelLoaded;
    }
    void OnDisable()
    {
        SceneLoader.Instance.LevelLoaded -= OnLevelLoaded;
    }

    public string GetLevel()
    {
        return m_Levels[CurrentLevel];
    }

    void FindFinish()
    {
        var finish = FindFirstObjectByType<LevelFinish>();
        if (finish == null)
        {
            DomainDebug.LogError($"Finish not found in level {CurrentLevel + 1}: {SceneLoader.Instance.Scene.name}", DomainType.Level);
            return;
        }
        finish.LevelFinished += OnLevelFinished;
    }

    void OnLevelLoaded()
    {
        FindFinish();
    }

    void OnLevelFinished()
    {
        LevelPreLoad?.Invoke();

        CurrentLevel++;
        if (CurrentLevel >= m_Levels.Count) EndedLevels?.Invoke();
        else SceneLoader.Instance.LoadLevel();
    }

    public void LoadLevel(int index)
    {
        if (index == -1) return;
        CurrentLevel = Mathf.Min(index, m_Levels.Count - 1);
    }
}
