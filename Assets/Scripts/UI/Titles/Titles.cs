using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titles : MonoBehaviour
{
    [SerializeField] HackingText m_Hack;
    [SerializeField] RectTransform[] m_Moving;
    [SerializeField] float m_Duration;

    void Start()
    {
        m_Hack.StartHack(m_Duration);
        StartCoroutine(MoveToZero(m_Moving));
    }

    IEnumerator MoveToZero(params RectTransform[] objs)
    {
        Timer timer = new(m_Duration);
        List<Vector2> starts = new(objs.Length);
        foreach (var obj in objs) starts.Add(obj.anchoredPosition);
        while (timer.Progress < 1)
        {
            for (int i = 0; i < objs.Length; ++i)
                objs[i].anchoredPosition = Vector2.Lerp(starts[i], Vector2.zero, timer.Progress);
            yield return null;
            timer.Update(Time.deltaTime);
        }
        foreach (var obj in objs) obj.anchoredPosition = Vector2.zero;

		yield return new WaitForSeconds(1);
        Ended();
    }

    void Ended()
    {
        GameManager.Instance.ToMenu();
    }
}
