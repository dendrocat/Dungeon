using UnityEngine;
using UnityEngine.Events;

public class LevelFinish : MonoBehaviour, IInteractable
{
	public UnityAction LevelFinished;

    public void Interact(Player _)
    {
		LevelFinished?.Invoke();	
    }
}
