using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamagable
{
    [SerializeField, Min(10)] float m_Health = 100;

    public float Health { get => m_Health; private set => m_Health = value; }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }

    void Die()
    {
        Debug.Log($"{name} died");
        Destroy(gameObject, 20);
    }
}
