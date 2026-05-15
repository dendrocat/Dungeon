using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ThanksMenu : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI m_Text;

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkId = TMP_TextUtilities.FindIntersectingLink(m_Text, eventData.position, eventData.pressEventCamera);
        if (linkId == -1) return;
        Application.OpenURL(m_Text.textInfo.linkInfo[linkId].GetLink());
    }
}
