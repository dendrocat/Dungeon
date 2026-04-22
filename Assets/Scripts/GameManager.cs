using UnityEngine;
using UnityEngine.SceneManagement;
using DomainLogging;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        var finish = FindFirstObjectByType<LevelFinish>();
        if (finish == null)
        {
            DomainDebug.LogError($"Finish not found", DomainType.Room);
            return;
        }
        finish.LevelFinished += OnLevelFinished;
    }


    void OnLevelFinished()
    {
		DomainDebug.Log($"Level finished", DomainType.Room);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
