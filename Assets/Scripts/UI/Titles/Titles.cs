using UnityEngine;
using DG.Tweening;

public class Titles : MonoBehaviour
{
    [SerializeField] HackingText m_Hack;
    [SerializeField] RectTransform[] m_Moving;
    [SerializeField, Min(0)] float m_Duration;
	[SerializeField] Ease m_Ease;

    void Start()
    {
        m_Hack.StartHack(m_Duration);

        Sequence seq = DOTween.Sequence();
        foreach (var mov in m_Moving)
            seq.Join(mov.DOAnchorPos(Vector3.zero, m_Duration).SetEase(m_Ease));

        seq.AppendInterval(1f);
        seq.AppendCallback(Ended);
    }

    void Ended()
    {
        GameManager.Instance.ToMenu();
    }
}
