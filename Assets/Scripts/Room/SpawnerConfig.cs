using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "SpawnerConfig", menuName = "Config/SpawnerConfig")]
public class SpawnerConfig : ScriptableObject
{

    [Serializable]
    public class ConfigEntry
    {
        [Required]
        public GameObject Prefab;

        [Slider(0f, 1f)]
        public float SpawnChance = 0.5f;
    }

    [TableList]
    [SerializeField] List<ConfigEntry> m_SpawnChances;

    public IReadOnlyList<ConfigEntry> SpawnChances => m_SpawnChances;

    [Button("Normalize chances")]
    void OnValidate()
    {
        if (m_SpawnChances == null && m_SpawnChances.Count == 0) return;

        float sum = m_SpawnChances.Sum(e => e.SpawnChance);
        if (!Mathf.Approximately(sum, 1) && sum > 0)
        {
            for (int i = 0; i < m_SpawnChances.Count; ++i)
                m_SpawnChances[i].SpawnChance = m_SpawnChances[i].SpawnChance / sum;
        }
    }

}
