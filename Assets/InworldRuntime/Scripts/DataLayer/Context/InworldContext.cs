using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Node;
using Inworld.Framework.STT;
using Inworld.Framework.TextEmbedder;
using Inworld.Framework.TTS;


namespace Inworld.Framework
{
    public interface IGetNodeHandler
    {
        GoalAdvancementNode GetGoalAdvancementNode(string componentID);
        IntentNode GetIntentNode(string componentID);
        KeywordMatcherNode GetKeywordMatcherNode(string componentID);
        KnowledgeNode GetKnowledgeNode(string componentID);
        LLMChatNode GetLLMChatNode(string componentID);
        LLMChatRequestBuilderNode GetLLMChatRequestBuilderNode(string componentID);
        LLMCompletionNode GetLLMCompletionNode(string componentID);
        LLMPromptBuilderNode GetLLMPromptBuilderNode(string componentID);
        MemoryUpdateNode GetMemoryUpdateNode(string componentID);
        MemoryRetrieveNode GetMemoryRetrieveNode(string componentID);
        RandomCannedTextNode GetRandomCannedTextNode(string componentID);
        SafetyCheckerNode GetSafetyCheckerNode(string componentID);
        STTNode GetSTTNode(string componentID);
        TextAggregatorNode GetTextAggregatorNode(string componentID);
        TextChunkingNode GetTextChunkingNode(string componentID);
        TTSNode GetTTSNode(string componentID);
        MCPListToolsNode GetMCPListToolsNode(string componentID);
        LLMInterface GetLLMInterface(string componentID);
        STTInterface GetSTTInterface(string componentID);
        TTSInterface GetTTSInterface(string componentID);
        TextEmbedderInterface GetTextEmbedderInterface(string componentID);
        KnowledgeInterface GetKnowledgeInterface(string componentID);
        MCPClientInterface GetMCPClientInterface(string componentID);
    }

    public interface IAddInterfaceHandler
    {
        bool AddLLMInterface(string componentID, LLMInterface llmInterface);
        bool AddSTTInterface(string componentID, STTInterface sttInterface);
        bool AddTTSInterface(string componentID, TTSInterface ttsInterface);
        bool AddTextEmbedderInterface(string componentID, TextEmbedderInterface textEmbedderInterface);
        bool AddKnowledgeInterface(string componentID, KnowledgeInterface knowledgeInterface);
        bool AddMCPClientInterface(string componentID, MCPClientInterface mcpClientInterface);
    }
    
    public class InworldContext : InworldFrameworkDllClass
    {
        
    }
}