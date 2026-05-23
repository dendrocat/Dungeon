using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        LevelManager.EndedLevels += ToTitles;
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

    public void ContinueGame()
    {
        SaveSystem.LoadData();
        SceneLoader.Instance.LoadLevel();
    }

    public void StartGame()
    {
        Repository.Remove();
        ContinueGame();
    }

    public void ToTitles()
    {
        SceneLoader.Instance.LoadScene("Titles");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}
