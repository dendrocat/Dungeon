using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button m_ContinueButton;

    void Awake()
    {
        if (!Repository.HasSave()) m_ContinueButton.gameObject.SetActive(false);
    }

    public void Continue() => GameManager.Instance.ToLevels();

    public void Start()
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
