using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using DomainLogging;
using PlayerPrefs = UnityEngine.PlayerPrefs;

public static class SettingsRepository
{
    const string c_KeyKeys = "SavedKeys";
    static Dictionary<string, object> m_TmpSettings = new();
    static Dictionary<string, object> m_Settings = new();

    static readonly Dictionary<Type, string> TypePrefixes = new() {
        { typeof(int), "int_" },
        { typeof(float), "float_" },
        { typeof(bool), "bool_" },
        { typeof(string), "string_" },
    };

    [UnityEngine.RuntimeInitializeOnLoadMethod]
    public static void Load()
    {
        string json = PlayerPrefs.GetString(c_KeyKeys, "");
        if (string.IsNullOrEmpty(json)) { m_Settings.Clear(); return; }

        IReadOnlyCollection<string> keys = JsonConvert.DeserializeObject<IReadOnlyCollection<string>>(json);
        foreach (string key in keys)
        {
            Type type = null;
            foreach (var kv in TypePrefixes)
                if (key.StartsWith(kv.Value))
                    type = kv.Key;

            object value;
            if (type == typeof(int)) value = PlayerPrefs.GetInt(key);
            else if (type == typeof(float)) value = PlayerPrefs.GetFloat(key);
            else if (type == typeof(bool)) value = PlayerPrefs.GetInt(key) != 0;
            else value = PlayerPrefs.GetString(key);

            m_Settings[key] = value;
        }
    }

    public static void Save()
    {
        foreach (var kv in m_TmpSettings) m_Settings[kv.Key] = kv.Value;
		m_TmpSettings.Clear();

        foreach (var kv in m_Settings)
        {
            if (kv.Value is int i) PlayerPrefs.SetInt(kv.Key, i);
            else if (kv.Value is float f) PlayerPrefs.SetFloat(kv.Key, f);
            else if (kv.Value is bool b) PlayerPrefs.SetInt(kv.Key, b ? 1 : 0);
            else if (kv.Value is string s) PlayerPrefs.SetString(kv.Key, s);
        }

        PlayerPrefs.SetString(c_KeyKeys, JsonConvert.SerializeObject(m_Settings.Keys));
        PlayerPrefs.Save();
    }

    public static void SetSetting<T>(string key, T value)
    {
        if (!TypePrefixes.TryGetValue(typeof(T), out string prefix))
        {
            DomainDebug.LogError($"Type {typeof(T)} is unsupported", DomainType.Save);
            return;
        }
        m_TmpSettings[prefix + key] = value;
    }

    public static T GetSetting<T>(string key, T defaultValue = default)
    {
        if (!TypePrefixes.TryGetValue(typeof(T), out string prefix))
        {
            DomainDebug.LogError($"Type {typeof(T)} is unsupported", DomainType.Save);
            return defaultValue;
        }
        return (T)m_Settings.GetValueOrDefault(key, defaultValue);
    }

    public static void Clear()
    {
        m_TmpSettings.Clear();
        m_Settings.Clear();
        PlayerPrefs.DeleteAll();
    }
}
