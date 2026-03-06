using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour, IDamagable
{
    [SerializeField] public UnityEvent Died = new();

    [SerializeField, Min(10)] float m_Health = 100;

    public float Health
    {
        get => m_Health;
        private set
        {
            m_Health = value;
            if (m_Health <= 0) Died.Invoke();
        }
    }


    public void TakeDamage(float damage)
    {
        m_Health -= damage;
    }
}

