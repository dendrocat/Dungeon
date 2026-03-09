using System;
using System.Collections.Generic;
using UnityEngine;
using TriInspector;

[CreateAssetMenu(fileName = "SpawnerConfig", menuName = "Config/SpawnerConfig")]
public class SpawnerConfig : ScriptableObject
{

    [Serializable]
    public struct ConfigEntry
    {
        public GameObject Prefab;

        [Slider(0.01f, 1f)]
        public float SpawnChance;
    }

    [SerializeField] List<ConfigEntry> m_SpawnChances;

    public IReadOnlyList<ConfigEntry> SpawnChances => m_SpawnChances;
}
