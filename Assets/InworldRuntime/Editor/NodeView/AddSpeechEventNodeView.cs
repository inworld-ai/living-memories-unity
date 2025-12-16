#if UNITY_EDITOR
using Inworld.Framework.Samples.Node;
using UnityEditor;
using UnityEngine.UIElements;

namespace Inworld.Framework.Editor
{
    public class AddSpeechEventNodeView : InworldNodeView
    {
        Toggle m_IsPlayerToggle;
        SerializedObject m_SO;

        protected override void OnNodeAssetAssigned()
        {
            if (!(NodeAsset is AddSpeechEventNodeAsset))
                return;
            m_SO = new SerializedObject(NodeAsset);
            if (m_IsPlayerToggle == null)
            {
                m_IsPlayerToggle = new Toggle("Is Player");
                mainContainer.Add(m_IsPlayerToggle);
            }
            SerializedProperty sp = m_SO.FindProperty("m_IsPlayer");
            if (sp != null)
            {
                m_IsPlayerToggle.SetValueWithoutNotify(sp.boolValue);
                m_IsPlayerToggle.RegisterValueChangedCallback(evt =>
                {
                    sp.boolValue = evt.newValue;
                    m_SO.ApplyModifiedProperties();
                });
            }
        }
    }
}
#endif