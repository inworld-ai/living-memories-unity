#if UNITY_EDITOR
using Inworld.Framework.Graph;
using UnityEngine.UIElements;

namespace Inworld.Framework.Editor
{
    public class TTSNodeView : InworldNodeView
    {
        TextField m_VoiceField;

        protected override void OnNodeAssetAssigned()
        {
            if (NodeAsset is not TTSNodeAsset tts)
                return;
            if (m_VoiceField == null)
            {
                m_VoiceField = new TextField("Voice ID:");
                mainContainer.Add(m_VoiceField);
            }
            m_VoiceField.SetValueWithoutNotify(tts.voiceID);
            m_VoiceField.RegisterValueChangedCallback(evt =>
            {
                tts.voiceID = evt.newValue;
            });
        }

        public override InworldNodeAsset SaveToAsset()
        {
            InworldNodeAsset asset = base.SaveToAsset();
            if (asset is TTSNodeAsset tts)
            {
                if (m_VoiceField != null)
                    tts.voiceID = m_VoiceField.value;
                ConfigData cfg = tts.ExecutionConfigData;
                if (cfg is TTSExecutionConfigPropData ttsCfg)
                {
                    ttsCfg.properties ??= new TTSExecutionPropertyData();
                    ttsCfg.properties.voice ??= new InworldVoicePropData();
                    ttsCfg.properties.voice.speakerID = tts.voiceID;
                }
            }
            return asset;
        }
    }
}
#endif