using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TriInspector;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; } = null;
    const float c_MinTime = 0.5f;

    [Scene]
    [SerializeField] string m_LevelScene;
    string m_LevelSceneName;

    AsyncOperation m_LevelOp = null, m_SceneOp = null;
    float m_Elapsed = 0;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LevelManager.LoadLevelStarted += OnLevelLoadStarted;

        m_LevelSceneName = System.IO.Path.GetFileNameWithoutExtension(m_LevelScene);
    }

    void Start()
    {
        LoadScene("TestLevelScene");
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    void OnLevelLoadStarted(AsyncOperation op)
    {
        Debug.Log("Level op recieved");
        StartCoroutine(LoadLevelAsync(op));
        // Task.WaitAll(m_LevelTask, Task.Delay(2000));
    }

    YieldInstruction Next()
    {
        m_Elapsed += Time.deltaTime;
        return null;
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        m_SceneOp = SceneManager.LoadSceneAsync(sceneName);
        m_SceneOp.allowSceneActivation = false;

        const float limit = 0.9f;
        while (m_SceneOp.progress < limit)
        {
            float progress = Mathf.Clamp01(m_SceneOp.progress / limit);
            Debug.Log(progress);
            yield return Next();
        }
        Debug.Log(m_SceneOp.progress);
        Debug.Log("Scene loaded");

        if (m_LevelScene.Contains(sceneName))
        {
            m_SceneOp.allowSceneActivation = true;
            m_SceneOp = null;
            Debug.Log("Scene activated from async");
        }
        else yield return StartCoroutine(WaitMinTime());

    }

    IEnumerator LoadLevelAsync(AsyncOperation op)
    {
        m_LevelOp = op;
        m_LevelOp.allowSceneActivation = false;

        const float limit = 0.9f;
        while (m_LevelOp.progress < limit)
        {
            Debug.Log(m_LevelOp.progress);
            yield return Next();
        }
        StartCoroutine(WaitMinTime());
    }

    IEnumerator WaitMinTime()
    {
        while (m_Elapsed < c_MinTime)
        {
            Debug.Log(m_Elapsed / c_MinTime);
            yield return Next();
        }
        if (m_SceneOp != null)
        {
            m_SceneOp.allowSceneActivation = true;
            m_SceneOp = null;
            Debug.Log("Scene activated");
        }
        if (m_LevelOp != null)
        {
            m_LevelOp.allowSceneActivation = true;
            m_LevelOp = null;
            Debug.Log("Level activated");
        }
        m_Elapsed = 0;
    }
}
