using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FinishAudio : MonoBehaviour
{
	public void Play() {
		GetComponent<AudioSource>().Play();
	}
}
