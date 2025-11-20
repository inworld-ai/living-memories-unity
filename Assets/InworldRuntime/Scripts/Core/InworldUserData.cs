using System.Collections.Generic;
using Inworld.Framework.Graph;
using Inworld.Framework.Intents;
using Newtonsoft.Json;
using UnityEngine;

namespace Inworld.Framework
{
    [CreateAssetMenu(fileName = "InworldUserData", menuName = "Inworld/User Data")]
    public class InworldUserData : ScriptableObject
    {
        public string apiKey;
        [Tooltip("Related to StreamingAssets")]
        public string sttModelPath;
        [Tooltip("Related to StreamingAssets")]
        public string textClassifierModelPath;
        [Tooltip("Related to StreamingAssets")]
        public string safetyModelPath;
        public string knowledgeID;
        public string agentName;
        public string playerName;
        public List<KeywordGroup> safetyKeywords;//
        public IntentData intents;//
        [TextArea] public string knowledgeFilterPrompt;
        [TextArea] public string dialogPrompt;
        [TextArea] public string relationPrompt;
        [TextArea] public string intentMatchingPromptTemplate;
        [TextArea] public string flashMemoryUpdaterPromptTemplate;
        [TextArea] public string ltmPromptTemplate;

        public InworldHashMap<string, string> ToHashMap
        {
            get
            {
                InworldHashMap<string, string> hashMap = new InworldHashMap<string, string>();
                hashMap["INWORLD_API_KEY"] = apiKey;
                hashMap["STT_MODEL_PATH"] = $"{Application.streamingAssetsPath}/{sttModelPath}";
                hashMap["TEXT_CLASSIFIER_MODEL_PATH"] = $"{Application.streamingAssetsPath}/{textClassifierModelPath}";
                hashMap["SAFETY_MODEL_PATH"] = $"{Application.streamingAssetsPath}/{safetyModelPath}";
                hashMap["KNOWLEDGE_ID"] = knowledgeID;
                hashMap["AGENT_NAME"] = agentName;
                hashMap["PLAYER_NAME"] = playerName;
                hashMap["SAFETY_KEYWORDS"] = JsonConvert.SerializeObject(safetyKeywords);
                hashMap["INTENTS"] = JsonConvert.SerializeObject(intents);
                hashMap["KNOWLEDGE_FILTER_PROMPT"] = knowledgeFilterPrompt;
                hashMap["DIALOG_PROMPT"] = dialogPrompt;
                hashMap["RELATION_PROMPT"] = relationPrompt;
                hashMap["INTENT_MATCHING_PROMPT_TEMPLATE"] = intentMatchingPromptTemplate;
                hashMap["FLASH_MEMORY_UPDATER_PROMPT_TEMPLATE"] = flashMemoryUpdaterPromptTemplate;
                hashMap["LTM_PROMPT_TEMPLATE"] = ltmPromptTemplate;
                return hashMap;
            }
        }
    }
}

