using System.Collections.Generic;

public static class Repository
{
    static Dictionary<string, object> currentState = new();

    public static void Load()
    {
		// load from file to dict
    }

    public static void Save()
    {
		// save from dict to file
    }

    public static bool SetData<K, V>(K key, V value)
    {
        return currentState.TryAdd(key.ToString(), value);
    }

    public static bool GetData<K, V>(K key, out V value)
    {
        bool res = currentState.TryGetValue(key.ToString(), out var val);
        if (res) value = (V)val;
        else value = default;
        return res;
    }
}
