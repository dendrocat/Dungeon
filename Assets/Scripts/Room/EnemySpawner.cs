using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public event UnityAction<IReadOnlyList<Enemy>> Spawned;

    [SerializeField] SpawnerConfig m_SpawnerConfig;

    void OnEnable()
    {
        Room.SpawnActivated += Spawn;
    }
    void OnDisable()
    {
        Room.SpawnActivated -= Spawn;
    }

    void Spawn(IProvider<Transform> spawnProvider)
    {

        var lst = new List<Enemy>(spawnProvider.Items.Count);
        foreach (var point in spawnProvider.Items)
        {
            float chance = Random.value;
            foreach (var entry in m_SpawnerConfig.SpawnChances)
            {
                if (chance < entry.SpawnChance)
                {
                    lst.Add(Instantiate(entry.Prefab, point.transform.position, Quaternion.identity).GetComponent<Enemy>());
                    break;
                }
                chance -= entry.SpawnChance;
            }
        }

        for (int i = 0; i < lst.Count; ++i)
            lst[i].name = $"{lst[i].name.Replace("(Clone)", "")}_{i + 1}";

        Spawned?.Invoke(lst);
    }
}
