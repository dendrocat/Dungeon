using TriInspector;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [ShowInInspector, Min(30)] public int Health { get; private set; } = 30;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }

    void Die()
    {
        Debug.Log($"{name} died");
        Destroy(gameObject);
    }
}
