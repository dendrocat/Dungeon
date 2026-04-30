using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        ToMenu();
    }

    public void ToMenu()
    {
        UnPause();
        SceneLoader.Instance.LoadScene("MainMenu");
    }

    public void ToLevels()
    {
        SceneLoader.Instance.LoadScene("TestLevelScene");
    }

    public void ToTitles()
    {
        SceneLoader.Instance.LoadScene("Titles");
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
