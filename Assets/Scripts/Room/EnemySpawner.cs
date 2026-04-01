using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public event UnityAction<IReadOnlyCollection<Enemy>> Spawned;

    [SerializeField] SpawnerConfig m_SpawnerConfig;

    void OnEnable()
    {
        Room.Activated += Spawn;
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

        for (int i = 0; i < lst.Count; ++i)
        {
            lst[i].name = $"{lst[i].name.Replace("(Clone)", "")}_{i + 1}";
            lst[i].SetWaypointProvider(room);
        }

        Spawned?.Invoke(lst);
    }
}
