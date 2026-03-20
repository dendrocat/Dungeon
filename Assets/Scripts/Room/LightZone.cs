using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class LightZone : MonoBehaviour
{
    public static event UnityAction<bool> PlayerInsideChanged;

    void OnPlayerInsideChanged(bool playerInside)
    {
        PlayerInsideChanged?.Invoke(playerInside);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) OnPlayerInsideChanged(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) OnPlayerInsideChanged(false);
    }
}
