using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
    }

    public void ToMenu()
    {
        SceneLoader.Instance.LoadScene("MainMenu");
    }

    public void ToLevels()
    {
        SceneLoader.Instance.LoadScene("Levels");
    }

    public void ToTitles()
    {
        SceneLoader.Instance.LoadScene("Titles");
    }

}
