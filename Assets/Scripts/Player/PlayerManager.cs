using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour, IDamagable
{
    [HideInInspector] UnityEvent Died = new();

    [SerializeField, Min(10)] int m_Health = 100;

    public int Health
    {
        get => m_Health;
        private set
        {
            m_Health = value;
            if (m_Health <= 0) Died.Invoke();
        }
    }


    public void TakeDamage(int damage)
    {
        m_Health -= damage;
    }
}