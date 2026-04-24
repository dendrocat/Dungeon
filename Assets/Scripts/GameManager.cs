using UnityEngine;
using UnityEngine.SceneManagement;
using DomainLogging;

public class GameManager : MonoBehaviour
{
    void Start()
    {
    }


    void OnLevelFinished()
    {
		DomainDebug.Log($"Level finished", DomainType.Room);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
