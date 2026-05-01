using System.Collections.Generic;
using DomainLogging;

using Path = System.IO.Path;
using File = System.IO.File;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using JObject = Newtonsoft.Json.Linq.JObject;

public static class Repository
{
    static Dictionary<string, object> m_CurrentState = new();

    const string c_FileName = "save.json";

    private static string GetPath() =>
        Path.Combine(UnityEngine.Application.persistentDataPath, c_FileName);

    public static bool HasSave() =>
        File.Exists(GetPath());

    public static void Remove() =>
        File.Delete(GetPath());

    public static void Load()
    {
        if (!HasSave()) return;
        DomainDebug.Log($"Loading from file: {GetPath()}", DomainType.Save);
        string data = File.ReadAllText(GetPath());
        DomainDebug.Log(data, DomainType.Save);
        m_CurrentState = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
    }

    public static void Save()
    {
        DomainDebug.Log($"Saving to file: {GetPath()}", DomainType.Save);
        string data = JsonConvert.SerializeObject(m_CurrentState);
        DomainDebug.Log(data, DomainType.Save);
        File.WriteAllText(GetPath(), data);
    }

    public static void SetData<T>(string key, T value)
    {
        m_CurrentState[key] = value;
    }

    public static bool GetData<T>(string key, out T value)
    {
        value = default(T);
        if (!m_CurrentState.TryGetValue(key.ToString(), out var valueObject)) return false;

        if (valueObject is T t) value = t;
        else if (valueObject is JObject jObj) m_CurrentState[key.ToString()] = value = jObj.ToObject<T>();
        else
        {
            DomainDebug.LogWarning($"Expected type: {valueObject.GetType()}, recieved type: {typeof(T)}. Value: {valueObject}", DomainType.Save);
            return false;
        }

        DomainDebug.Log($"Value for {key}: {value}", DomainType.Save);
        return true;
    }

    public static void ClearCache()
    {
        m_CurrentState.Clear();
    }
}
