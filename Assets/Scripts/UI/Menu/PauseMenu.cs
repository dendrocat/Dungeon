using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    InputManager m_Input;
    void Start()
    {
        m_Input = GetComponentInParent<Player>().Input;
        m_Input.Paused += ChangeState;
        gameObject.SetActive(false);
    }

    void ChangeState()
    {
        if (gameObject.activeSelf) GameManager.Instance.UnPause();
        else GameManager.Instance.Pause();
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void Continue()
    {
        m_Input.SwitchMap(InputManager.ActionMap.Player);
        ChangeState();
    }

    public void ToMenu() => GameManager.Instance.ToMenu();
}
