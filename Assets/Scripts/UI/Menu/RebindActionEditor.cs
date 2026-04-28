#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

namespace UnityEngine.InputSystem.Rebinding
{

	[CustomEditor(typeof(RebindAction))]
    public class RebindActionEditor : UnityEditor.Editor
    {
        static class Styles
        {
            public static GUIStyle boldLabel = new GUIStyle("BoldLabel");
        }

        SerializedProperty m_ActionProperty;
        SerializedProperty m_BindingIdProperty;
		SerializedProperty m_DisplayStringOptionsProperty;
        SerializedProperty m_BindingTextProperty;

        GUIContent m_BindingLabel = new GUIContent("Binding");
        GUIContent m_DisplayOptionsLabel = new GUIContent("Display Options");
        GUIContent m_UILabel = new GUIContent("UI");

        GUIContent[] m_BindingOptions;
        string[] m_BindingOptionValues;
        int m_SelectedBindingOption;

        void OnEnable()
        {
            m_ActionProperty = serializedObject.FindProperty("m_Action");
            m_BindingIdProperty = serializedObject.FindProperty("m_BindingId");
            m_DisplayStringOptionsProperty = serializedObject.FindProperty("m_DisplayStringOptions");
			m_BindingTextProperty = serializedObject.FindProperty("m_BindingText");

            RefreshBindingOptions();
        }

        void RefreshBindingOptions()
        {
            var actionProperty = (InputActionProperty)m_ActionProperty.boxedValue;
            if (actionProperty == null)
            {
                m_BindingOptions = new GUIContent[0];
                m_BindingOptionValues = new string[0];
                m_SelectedBindingOption = -1;
                return;
            }
            var action = actionProperty.action;
            var bindings = action.bindings;

            m_BindingOptions = new GUIContent[bindings.Count];
            m_BindingOptionValues = new string[bindings.Count];
            m_SelectedBindingOption = -1;

            var currentBindingId = m_BindingIdProperty.stringValue;
            for (var i = 0; i < bindings.Count; ++i)
            {
                var binding = bindings[i];
                var bindingId = binding.id.ToString();
                var haveBindingGroups = !string.IsNullOrEmpty(binding.groups);

                var displayOptions =
                    InputBinding.DisplayStringOptions.DontUseShortDisplayNames | InputBinding.DisplayStringOptions.IgnoreBindingOverrides;
                if (!haveBindingGroups)
                    displayOptions |= InputBinding.DisplayStringOptions.DontOmitDevice;

                var displayString = action.GetBindingDisplayString(i, displayOptions);

                // If binding is part of a composite, include the part name.
                if (binding.isPartOfComposite)
                    displayString = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayString}";

                displayString = displayString.Replace('/', '\\');

                // If the binding is part of control schemes, mention them.
                if (haveBindingGroups)
                {
                    var asset = action.actionMap?.asset;
                    if (asset != null)
                    {
                        var controlSchemes = string.Join(", ",
                            binding.groups.Split(InputBinding.Separator)
                                .Select(x => asset.controlSchemes.FirstOrDefault(c => c.bindingGroup == x).name));

                        displayString = $"{displayString} ({controlSchemes})";
                    }
                }

                m_BindingOptions[i] = new GUIContent(displayString);
                m_BindingOptionValues[i] = bindingId;

                if (currentBindingId == bindingId)
                    m_SelectedBindingOption = i;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            // Binding section.
            EditorGUILayout.LabelField(m_BindingLabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(m_ActionProperty);

                var newSelectedBinding = EditorGUILayout.Popup(m_BindingLabel, m_SelectedBindingOption, m_BindingOptions);
                if (newSelectedBinding != m_SelectedBindingOption)
                {
                    var bindingId = m_BindingOptionValues[newSelectedBinding];
                    m_BindingIdProperty.stringValue = bindingId;
                    m_SelectedBindingOption = newSelectedBinding;
                }

                var optionsOld = (InputBinding.DisplayStringOptions)m_DisplayStringOptionsProperty.intValue;
                var optionsNew = (InputBinding.DisplayStringOptions)EditorGUILayout.EnumFlagsField(m_DisplayOptionsLabel, optionsOld);
                if (optionsOld != optionsNew)
                    m_DisplayStringOptionsProperty.intValue = (int)optionsNew;
            }

            // UI section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(m_UILabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(m_BindingTextProperty);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                RefreshBindingOptions();
            }
        }
    }
}
#endif
