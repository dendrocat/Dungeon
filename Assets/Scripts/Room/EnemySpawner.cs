using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SpawnerConfig m_SpawnerConfig;

    public IReadOnlyList<Enemy> Spawn()
    {
        var lst = new List<Enemy>();
        // TODO: logic
        return lst;
    }
}
