using UnityEngine;

public class BonusSound : MonoBehaviour
{
	[SerializeField] AudioSource m_Source;

	public void Play() => m_Source.Play();
}
