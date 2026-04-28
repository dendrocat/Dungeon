using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button m_ContinueButton;

    void Awake()
    {
        if (!Repository.HasSave()) m_ContinueButton.gameObject.SetActive(false);
    }

    public void ContinueGame() => GameManager.Instance.ToLevels();

    public void StartGame()
    {
        Repository.Remove();
        GameManager.Instance.ToLevels();
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
