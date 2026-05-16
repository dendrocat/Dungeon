using UnityEngine;
using TriInspector;

[RequireComponent(typeof(Collider))]
public class BasicAudioEmitter : MonoBehaviour, IAudioEmitter
{
    [SerializeField, Slider(0.01f, 100f)] float m_Volume = 1f;
	[SerializeField, Min(0)] float m_DestroyTime = 0;

	void Awake() {
		if (m_DestroyTime == 0) return;
		Destroy(gameObject, m_DestroyTime);
	}

    public float GetAudioLevel()
    {
        return m_Volume;
    }
}
