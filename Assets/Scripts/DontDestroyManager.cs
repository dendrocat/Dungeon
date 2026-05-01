using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    void Awake()
    {
        while (transform.childCount > 0)
        {
            var obj = transform.GetChild(0);
            obj.SetParent(null, worldPositionStays: true);
            DontDestroyOnLoad(obj.gameObject);
        }
    }
}
