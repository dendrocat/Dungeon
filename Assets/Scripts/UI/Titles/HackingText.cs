using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using TextUI = TMPro.TextMeshProUGUI;

[RequireComponent(typeof(TextUI))]
public class HackingText : MonoBehaviour
{
    const int c_MinChanges = 2000;
    TextUI m_Text;
    [SerializeField] float m_ChangeInterval = 0.1f;

    const string c_Specials = "!$*(&_+-^%)#@ ";

    void Awake()
    {
        m_Text = GetComponent<TextUI>();
    }

    public void StartHack(float duration)
    {
        StartCoroutine(HackText(m_Text.text, duration));
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

    System.Collections.IEnumerator HackText(string target, float duration)
    {
        List<int> indexes = Enumerable.Range(0, target.Length).ToList();
        StringBuilder hacking = new(target);
        for (int i = 0; i < target.Length; ++i)
        {
            do hacking[i] = GetSymbol();
            while (hacking[i] == target[i]);
        }
        m_Text.text = hacking.ToString();

        ShuffleIndexes(indexes);

        Timer timer = new(duration);
        int hacked = 0;
        while (timer.Progress < 1)
        {
            int targetHacked = Mathf.FloorToInt(target.Length * timer.Progress);

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
            yield return new WaitForSeconds(m_ChangeInterval);

            timer.Update(m_ChangeInterval);
        }

        m_Text.text = target;
    }
}
