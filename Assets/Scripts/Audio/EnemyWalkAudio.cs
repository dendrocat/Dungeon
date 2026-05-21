using UnityEngine;

public class EnemyWalkAudio : MonoBehaviour
{
    [SerializeField] Enemy m_Enemy;

    [Space(15)]
    [SerializeField] AudioSource m_Source;
    [SerializeField] AudioClip m_WalkClip;
    [SerializeField] AudioClip m_RunClip;


    void Update()
    {
        if (m_Enemy.NavAgent.velocity.magnitude < 1f) m_Source.Stop();
        else
        {
			var maxSpeed = m_Enemy.NavAgent.speed;
            if (maxSpeed > m_Enemy.Config.Speed.BaseSpeed)
                m_Source.clip = m_RunClip;
            else
                m_Source.clip = m_WalkClip;

            if (!m_Source.isPlaying)
                m_Source.Play();
        }
    }
}
