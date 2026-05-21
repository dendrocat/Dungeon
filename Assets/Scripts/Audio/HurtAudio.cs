using UnityEngine;

public class HurtAudio : MonoBehaviour
{
    [SerializeField] AudioSource m_Hurt;

    void Awake()
    {
        GetComponentInParent<Person>().Attacked += OnAttacked;
    }

    void OnAttacked()
    {
        m_Hurt.Play();
    }
}
