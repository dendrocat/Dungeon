using UnityEngine;
using UnityEngine.Events;
using TriInspector;
using DomainLogging;

public class LevelManager : MonoBehaviour
{
	public static event UnityAction<AsyncOperation> LoadLevelStarted;
    public event UnityAction EndedLevels;

    [AssetsOnly]
    [SerializeField] GameObject[] m_Levels;

    int m_CurrentLevel = 0;
    GameObject m_LevelInstance;

    void Start()
    {
        if (m_Levels.Length == 0)
        {
            DomainDebug.LogError($"Levels not setted", DomainType.Level);
            return;
        }
        LoadNextLevel();
    }

    async void LoadNextLevel()
    {
        var op = InstantiateAsync(m_Levels[m_CurrentLevel], Vector3.zero, Quaternion.identity);
		LoadLevelStarted?.Invoke(op);
		m_LevelInstance = (await op)[0];
		OnLevelLoaded();
    }

    void OnLevelLoaded()
    {
        DomainDebug.Log($"Level {m_CurrentLevel + 1} instanced {m_LevelInstance.name}", DomainType.Level);
        var finish = FindFirstObjectByType<LevelFinish>();
        if (finish == null)
        {
            DomainDebug.LogError($"Finish not found in level {m_CurrentLevel + 1}: {m_LevelInstance.name}", DomainType.Level);
            return;
        }
        finish.LevelFinished += OnLevelFinished;
    }

    void OnLevelFinished()
    {
        Destroy(m_LevelInstance);
        m_CurrentLevel++;
        if (m_CurrentLevel >= m_Levels.Length) EndedLevels?.Invoke();
        else LoadNextLevel();
    }

}
