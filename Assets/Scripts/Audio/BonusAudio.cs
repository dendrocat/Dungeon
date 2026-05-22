using UnityEngine;

public class BonusAudio : MonoBehaviour
{
	[SerializeField] AudioSource m_Source;

	public void Play() => m_Source.Play();
}
