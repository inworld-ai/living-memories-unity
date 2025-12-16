using Inworld.Framework.Intents;
using Inworld.Framework.Node;
using Inworld.Framework.TextEmbedder;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    // Input: InworldText
    // Output: MatchedIntents
    [CreateAssetMenu(fileName = "Node_Intent", menuName = "Inworld/Create Node/Intent", order = -2796)]
    public class IntentNodeAsset : InworldNodeAsset
    {
        [SerializeField] LLMComponentAsset m_LLMComponent;
        [SerializeField] IntentData m_IntentData;
        public override string NodeTypeName => "IntentNode";
        
        public override NodeCreationConfig GetNodeCreationConfig()
        {
            IntentNodeCreationConfig nodeCreationCfg = new IntentNodeCreationConfig();
            nodeCreationCfg.EmbedderComponentID = ComponentID;
            if (!string.IsNullOrEmpty(m_LLMComponent?.ID))
                nodeCreationCfg.LLMComponentID = m_LLMComponent.ID;
            if (m_IntentData)
                nodeCreationCfg.Intents = m_IntentData.CreateRuntime();
            m_CreationConfig = nodeCreationCfg;
            return m_CreationConfig;
        }


        public override NodeExecutionConfig GetNodeExecutionConfig()
        {
            IntentNodeExecutionConfig executionConfig = new IntentNodeExecutionConfig();
            IntentMatcherConfig intentMatcherConfig = new IntentMatcherConfig();
            intentMatcherConfig.EmbeddingMatcherConfig = new EmbeddingMatcherConfig
            {
                SimilarityThreshold = 0.6f 
            };
            executionConfig.MatcherConfig = intentMatcherConfig;
            m_ExecutionConfig = executionConfig;
            return m_ExecutionConfig;
        }
        
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            ComponentStore componentStore = new ComponentStore();
            componentStore.AddTextEmbedderInterface(NodeName, InworldController.TextEmbedder.Interface as TextEmbedderInterface);
            InworldCreationContext creationContext = new InworldCreationContext(componentStore);
            IntentNodeCreationConfig creationCfg = GetNodeCreationConfig() as IntentNodeCreationConfig;
            IntentNodeExecutionConfig executionCfg = GetNodeExecutionConfig() as IntentNodeExecutionConfig;
            Runtime = new IntentNode(NodeName, creationContext, creationCfg, executionCfg);
            return Runtime?.IsValid ?? false;
        }
        
        public override ComponentData ComponentData => new ComponentData
        {
            id = NodeName,
            type = NodeTypeName,
            creationConfig = CreationConfigData,
            executionConfig = ExecutionConfigData
        };
    }
}