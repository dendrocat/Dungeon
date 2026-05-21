using UnityEngine;

public class WeaponAudio : MonoBehaviour, IWeaponActions
{
    [SerializeField] AudioSource m_Shoot;
    [SerializeField] AudioSource m_Reload;
    [SerializeField] AudioSource m_Equip;

    void Play(AudioSource source)
    {
		source.Play();
    }

    public void Attack()
    {
		m_Reload.Stop();
		m_Equip.Stop();
        Play(m_Shoot);
    }

    public void Reload()
    {
        Play(m_Reload);
    }

    public void Equip()
    {
        Play(m_Equip);
    }

    public void Unequip() { }
}
