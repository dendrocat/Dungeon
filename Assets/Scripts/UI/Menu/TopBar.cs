using UnityEngine;
using Button = UnityEngine.UI.Button;

public class TopBar : MonoBehaviour
{
    [System.Serializable]
    class ButtonView : IActivatable
    {
        public Button Button;
        public GameObject View;

        public bool IsActive => View.activeSelf;

        public void Init()
        {
            if (IsActive) Activate();
            else Deactivate();
        }

        public void Activate()
        {
            Button.interactable = false;
            View.SetActive(true);
        }

        public void Deactivate()
        {
            Button.interactable = true;
            View.SetActive(false);
        }
    }
	[TriInspector.TableList]
    [SerializeField] ButtonView[] m_ButtonViews;

    void Awake()
    {
        foreach (var view in m_ButtonViews)
        {
            view.Init();
            view.Button.onClick.AddListener(() => OnClick(view));
        }
    }

    void OnClick(ButtonView view)
    {
        foreach (var buttonView in m_ButtonViews)
            buttonView.Deactivate();
        view.Activate();
    }
}
