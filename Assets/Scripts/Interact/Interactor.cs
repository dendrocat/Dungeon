using UnityEngine;

public class Interactor : MonoBehaviour
{
    Player m_Player;
    void Start()
    {
        m_Player = GetComponent<Player>();
    }

    void OnEnable()
    {
        m_Player.Input.Interacted += OnInteracted;
    }

    void OnInteracted()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (!Physics.Raycast(ray, out var hit)) return;
        if (!hit.transform.TryGetComponent<IInteractable>(out var obj)) return;
        obj.Interact(m_Player);
    }

    void OnDisable()
    {
        m_Player.Input.Interacted -= OnInteracted;
    }
}
