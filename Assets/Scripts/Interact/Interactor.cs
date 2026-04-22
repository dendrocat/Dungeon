using UnityEngine;
using DomainLogging;

[RequireComponent(typeof(Player))]
public class Interactor : MonoBehaviour
{
    [SerializeField] LayerMask m_Mask;
    const float c_Radius = 5;
    Player m_Player;

    void Start()
    {
        m_Player = GetComponent<Player>();
        m_Player.Input.Interacted += OnInteracted;
    }

    void OnInteracted()
    {
        Collider[] c = Physics.OverlapSphere(transform.position, c_Radius, m_Mask);
		if (c.Length != 1) return;

        if (!c[0].TryGetComponent<IInteractable>(out var obj)) return;
        obj.Interact(m_Player);
        DomainDebug.Log($"Interacted with {obj}", DomainType.Player);
    }
}
