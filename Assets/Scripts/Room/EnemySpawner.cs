using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public event UnityAction<IReadOnlyCollection<Enemy>> Spawned;

    [SerializeField] SpawnerConfig m_SpawnerConfig;
    int m_CntSpawned = 0;

    void OnEnable()
    {
        Room.Activated += Spawn;
    }

    void OnDisable()
    {
        Room.Activated -= Spawn;
    }

    void Spawn(Room room)
    {
        var lst = new List<Enemy>(room.SpawnPoints.Count);
        foreach (var point in room.SpawnPoints)
        {
            float chance = Random.value;
            foreach (var entry in m_SpawnerConfig.SpawnChances)
            {
                if (chance < entry.SpawnChance)
                {
                    lst.Add(Instantiate(entry.Prefab, point.transform.position, Quaternion.identity, room.transform).GetComponent<Enemy>());
                    break;
                }
                chance -= entry.SpawnChance;
            }
        }

        if (m_CntSpawned + lst.Count > int.MaxValue) m_CntSpawned = 0;
        foreach (var enemy in lst)
        {
            int idx = ++m_CntSpawned;
            // deleting "(Clone)" (length 7) ending
            const int suff = 7;
            enemy.name = $"{enemy.name.Substring(0, enemy.name.Length - suff)}_{idx}";
            enemy.SetWaypointProvider(room);
        }

        Spawned?.Invoke(lst);
    }
}
