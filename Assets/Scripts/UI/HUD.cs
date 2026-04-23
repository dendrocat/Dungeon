using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] Player m_Player;

    [TriInspector.PropertySpace(10)]
    [SerializeField] HUDComponent[] m_Components;

    void Awake()
    {
		foreach (var cmp in m_Components)
			cmp.Init(m_Player);
    }
}
