using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

using TextUI = TMPro.TextMeshProUGUI;

[RequireComponent(typeof(TextUI))]
public class HackingText : MonoBehaviour
{
    TextUI m_Text;
    [SerializeField] Ease m_Ease;
    [SerializeField] float m_ChangeInterval = 0.1f;

    const string c_Specials = "!$*(&_+-^%)#@ ";

    void Awake()
    {
        m_Text = GetComponent<TextUI>();
    }

    public void StartHack(float duration)
    {
        HackText(m_Text.text, duration);
    }

    char GetSymbol()
    {
        var type = Random.Range(0, 6);
        return type switch
        {
            0 => (char)Random.Range('а', 'я' + 1),
            1 => (char)Random.Range('А', 'Я' + 1),
            2 => (char)Random.Range('a', 'z' + 1),
            3 => (char)Random.Range('A', 'Z' + 1),
            4 => (char)Random.Range('0', '9' + 1),
            5 => c_Specials[Random.Range(0, c_Specials.Length)],
            _ => '?',
        };
    }

    void ShuffleIndexes(in List<int> array)
    {
        for (int i = array.Count - 1; i > 0; --i)
        {
            int idx = Random.Range(0, i + 1);
            (array[i], array[idx]) = (array[idx], array[i]);
        }
    }

    void HackText(string target, float duration)
    {
        System.Text.StringBuilder hacking = new(target);
        for (int i = 0; i < target.Length; ++i)
        {
            do hacking[i] = GetSymbol();
            while (hacking[i] == target[i]);
        }
        m_Text.text = hacking.ToString();

        List<int> indexes = Enumerable.Range(0, target.Length).ToList();
        ShuffleIndexes(indexes);

        int hacked = 0;
        float time = Time.time;

        DOVirtual.Float(0, 1, duration, progress =>
            {
                if (Time.time - time < m_ChangeInterval) return;
                time = Time.time;

                int targetHacked = Mathf.Min(Mathf.FloorToInt(target.Length * progress), target.Length - 1);

                while (hacked < targetHacked)
                {
                    int idx = indexes[hacked++];
                    hacking[idx] = target[idx];
                }

                for (int i = hacked; i < target.Length; ++i)
                {
                    int idx = indexes[i];
                    do hacking[idx] = GetSymbol();
                    while (hacking[idx] == target[idx]);
                }

                m_Text.text = hacking.ToString();
            }
        )
        .SetEase(m_Ease)
        .OnComplete(() => m_Text.text = target);
    }
}
