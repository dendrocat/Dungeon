using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TriInspector;

public class SceneLoader : MonoBehaviour
{
    public UnityAction SceneLoaded;

    public static SceneLoader Instance { get; private set; } = null;
    const float c_MinTime = 2f;

    [SerializeField] LoadUI m_LoadUI;
    [Scene]
    [SerializeField] string m_LevelScene;

    float m_Elapsed = 0;
    float progress => Mathf.Clamp01(m_Elapsed / c_MinTime);

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); Destroy(m_LoadUI?.gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_LoadUI.gameObject);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        // Debug.Log(m_LevelSceneName);
        LoadScene("TestLevelScene");
    }

    public void LoadScene(string sceneName)
    {
        var op = Addressables.LoadSceneAsync(sceneName, activateOnLoad: false);
        StartCoroutine(LoadAsync(op,
                    async (res) =>
                        {
                            await op.Result.ActivateAsync();
                            SceneLoaded?.Invoke();
                        },
                    !m_LevelScene.Contains(sceneName))
        );
    }

    public void LoadLevel(AssetReferenceGameObject assetRef, UnityAction<GameObject> onLoaded)
    {
        // Debug.Log($"Level recieved");
        var op = assetRef.LoadAssetAsync();
        StartCoroutine(LoadAsync(op, onLoaded));
    }

    YieldInstruction Next()
    {
        m_Elapsed += Time.deltaTime;
        return null;
    }

    IEnumerator LoadAsync<T>(AsyncOperationHandle<T> op, UnityAction<T> onLoaded, bool waitMinTime = true)
    {
        m_LoadUI.Activate();
        // Debug.Log("Operation started");

        while (!op.IsDone)
        {
            m_LoadUI.ShowProgress(Mathf.Min(progress, op.PercentComplete));
            yield return Next();
        }
        if (waitMinTime)
            yield return StartCoroutine(WaitMinTime());

        onLoaded?.Invoke(op.Result);
        op.Release();
        // Debug.Log("Operation ended");

        m_LoadUI.Deactivate();
    }

    IEnumerator WaitMinTime()
    {
        while (m_Elapsed < c_MinTime)
        {
            m_LoadUI.ShowProgress(progress);
            yield return Next();
        }
        m_Elapsed = 0;
    }
}
