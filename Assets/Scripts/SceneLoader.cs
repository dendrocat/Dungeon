using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

using Scene = UnityEngine.SceneManagement.Scene;
using UnityAction = UnityEngine.Events.UnityAction;

[RequireComponent(typeof(LevelManager))]
public class SceneLoader : MonoBehaviour
{
    public UnityAction LevelLoaded;

    public static SceneLoader Instance { get; private set; } = null;
    const float c_MinLoadTime = 1f;

    AsyncOperationHandle<SceneInstance> m_SceneHandle;
    public Scene Scene => m_SceneHandle.IsValid() ? m_SceneHandle.Result.Scene : default;

    [SerializeField] LoadUI m_LoadUI;
    LevelManager m_LevelManager;

    float m_Elapsed = 0;
    float progress => Mathf.Clamp01(m_Elapsed / c_MinLoadTime);

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); Destroy(m_LoadUI?.gameObject); return;
        }

        Instance = this;
        m_LoadUI.Deactivate();

        m_LevelManager = GetComponent<LevelManager>();
    }

    void OnDestroy()
    {
        if (Instance = this) Instance = null;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, null));
    }

    public void LoadLevel()
    {
        var sceneName = m_LevelManager.GetLevel();
        StartCoroutine(LoadSceneAsync(sceneName, LevelLoaded));
    }

    YieldInstruction Next()
    {
        m_Elapsed += Time.deltaTime;
        return null;
    }

    IEnumerator LoadSceneAsync(string sceneName, UnityAction onLoaded)
    {
        m_LoadUI.Activate();
        // Debug.Log("Operation started");

        var op_dep = Addressables.DownloadDependenciesAsync(sceneName);
        yield return StartCoroutine(WaitOp(op_dep));

        m_SceneHandle = Addressables.LoadSceneAsync(sceneName, activateOnLoad: false);
        yield return StartCoroutine(WaitOp(m_SceneHandle));

        yield return StartCoroutine(WaitMinTime());

        var op_act = m_SceneHandle.Result.ActivateAsync();
        while (!op_act.isDone) yield return null;
        // Debug.Log("Operation ended");

        onLoaded?.Invoke();

        m_LoadUI.Deactivate();
    }

    IEnumerator WaitOp(AsyncOperationHandle op)
    {
        while (!op.IsDone)
        {
            m_LoadUI.ShowProgress(Mathf.Min(progress, op.PercentComplete));
            yield return Next();
        }
    }

    IEnumerator WaitMinTime()
    {
        while (m_Elapsed < c_MinLoadTime)
        {
            m_LoadUI.ShowProgress(progress);
            yield return Next();
        }
        m_Elapsed = 0;
    }
}
