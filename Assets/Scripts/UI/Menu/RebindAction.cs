namespace UnityEngine.InputSystem.Rebinding
{
    using TextUI = TMPro.TextMeshProUGUI;

    public class RebindAction : MonoBehaviour
    {
        const string c_BindingText = "Нажмите кнопку...";
        [SerializeField] InputActionReference m_Action;
        [SerializeField] InputBinding m_BindingMask;

        [SerializeField] int m_BindingIndex;

        [SerializeField] InputBinding.DisplayStringOptions m_DisplayStringOptions;

        [SerializeField] TextUI m_BindingText;

        void OnEnable()
        {
            UpdateBindingDisplay();
            InputSystem.onActionChange += OnActionChange;
        }

        void OnDisable()
        {
            InputSystem.onActionChange -= OnActionChange;
        }

        private void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.BoundControlsChanged) return;
            UpdateBindingDisplay();
        }

        void AfterRebind(InputActionRebindingExtensions.RebindingOperation op)
        {
            op.action.Enable();
            op.Dispose();
        }

        public void StartRebind()
        {
            var action = m_Action.action;

            action.Disable();
            // Debug.Log("Start Rebinding");
            m_BindingText.text = c_BindingText;
            action.PerformInteractiveRebinding(m_BindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .OnComplete(AfterRebind)
                .OnCancel(AfterRebind)
                .Start();
        }

        void OnValidate()
        {
            UpdateBindingDisplay();
        }

        void UpdateBindingDisplay()
        {
            var action = m_Action?.action;
            if (m_BindingText == null)
            {
                Debug.LogError($"Binding Text not setted\n{GetType()}");
                return;
            }
            if (action == null)
            {
                Debug.LogError($"Action not setted\n{GetType()}");
                m_BindingText.text = "None";
                return;
            }
            m_BindingText.text = action?.GetBindingDisplayString(m_BindingIndex, m_DisplayStringOptions) ?? string.Empty;
        }
    }
}
