using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Node;
using Inworld.Framework.STT;
using Inworld.Framework.TextEmbedder;
using Inworld.Framework.TTS;


namespace Inworld.Framework
{
    /// <summary>
    /// Provides getter methods to retrieve node instances and runtime interfaces by component ID.
    /// Implementations should resolve IDs to concrete graph nodes or service interfaces.
    /// </summary>
    public interface IGetNodeHandler
    {
        /// <summary>
        /// Gets the <see cref="GoalAdvancementNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="GoalAdvancementNode"/>, or null if not found.</returns>
        GoalAdvancementNode GetGoalAdvancementNode(string componentID);
        /// <summary>
        /// Gets the <see cref="IntentNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="IntentNode"/>, or null if not found.</returns>
        IntentNode GetIntentNode(string componentID);
        /// <summary>
        /// Gets the <see cref="KeywordMatcherNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="KeywordMatcherNode"/>, or null if not found.</returns>
        KeywordMatcherNode GetKeywordMatcherNode(string componentID);
        /// <summary>
        /// Gets the <see cref="KnowledgeNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="KnowledgeNode"/>, or null if not found.</returns>
        KnowledgeNode GetKnowledgeNode(string componentID);
        /// <summary>
        /// Gets the <see cref="LLMChatNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="LLMChatNode"/>, or null if not found.</returns>
        LLMChatNode GetLLMChatNode(string componentID);
        /// <summary>
        /// Gets the <see cref="LLMChatRequestBuilderNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="LLMChatRequestBuilderNode"/>, or null if not found.</returns>
        LLMChatRequestBuilderNode GetLLMChatRequestBuilderNode(string componentID);
        /// <summary>
        /// Gets the <see cref="LLMCompletionNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="LLMCompletionNode"/>, or null if not found.</returns>
        LLMCompletionNode GetLLMCompletionNode(string componentID);
        /// <summary>
        /// Gets the <see cref="LLMPromptBuilderNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="LLMPromptBuilderNode"/>, or null if not found.</returns>
        LLMPromptBuilderNode GetLLMPromptBuilderNode(string componentID);
        /// <summary>
        /// Gets the <see cref="MemoryUpdateNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="MemoryUpdateNode"/>, or null if not found.</returns>
        MemoryUpdateNode GetMemoryUpdateNode(string componentID);
        /// <summary>
        /// Gets the <see cref="MemoryRetrieveNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="MemoryRetrieveNode"/>, or null if not found.</returns>
        MemoryRetrieveNode GetMemoryRetrieveNode(string componentID);
        /// <summary>
        /// Gets the <see cref="RandomCannedTextNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="RandomCannedTextNode"/>, or null if not found.</returns>
        RandomCannedTextNode GetRandomCannedTextNode(string componentID);
        /// <summary>
        /// Gets the <see cref="SafetyCheckerNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="SafetyCheckerNode"/>, or null if not found.</returns>
        SafetyCheckerNode GetSafetyCheckerNode(string componentID);
        /// <summary>
        /// Gets the <see cref="STTNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="STTNode"/>, or null if not found.</returns>
        STTNode GetSTTNode(string componentID);
        /// <summary>
        /// Gets the <see cref="TextAggregatorNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="TextAggregatorNode"/>, or null if not found.</returns>
        TextAggregatorNode GetTextAggregatorNode(string componentID);
        /// <summary>
        /// Gets the <see cref="TextChunkingNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="TextChunkingNode"/>, or null if not found.</returns>
        TextChunkingNode GetTextChunkingNode(string componentID);
        /// <summary>
        /// Gets the <see cref="TTSNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="TTSNode"/>, or null if not found.</returns>
        TTSNode GetTTSNode(string componentID);
        /// <summary>
        /// Gets the <see cref="MCPListToolsNode"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="MCPListToolsNode"/>, or null if not found.</returns>
        MCPListToolsNode GetMCPListToolsNode(string componentID);
        /// <summary>
        /// Gets the <see cref="LLMInterface"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="LLMInterface"/>, or null if not found.</returns>
        LLMInterface GetLLMInterface(string componentID);
        /// <summary>
        /// Gets the <see cref="STTInterface"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="STTInterface"/>, or null if not found.</returns>
        STTInterface GetSTTInterface(string componentID);
        /// <summary>
        /// Gets the <see cref="TTSInterface"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="TTSInterface"/>, or null if not found.</returns>
        TTSInterface GetTTSInterface(string componentID);
        /// <summary>
        /// Gets the <see cref="TextEmbedderInterface"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="TextEmbedderInterface"/>, or null if not found.</returns>
        TextEmbedderInterface GetTextEmbedderInterface(string componentID);
        /// <summary>
        /// Gets the <see cref="KnowledgeInterface"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="KnowledgeInterface"/>, or null if not found.</returns>
        KnowledgeInterface GetKnowledgeInterface(string componentID);
        /// <summary>
        /// Gets the <see cref="MCPClientInterface"/> associated with the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <returns>The matching <see cref="MCPClientInterface"/>, or null if not found.</returns>
        MCPClientInterface GetMCPClientInterface(string componentID);
    }

    /// <summary>
    /// Provides methods to register runtime interfaces into the context by component ID.
    /// Returns a boolean that indicates whether the registration succeeded.
    /// </summary>
    public interface IAddInterfaceHandler
    {
        /// <summary>
        /// Adds or replaces the <see cref="LLMInterface"/> for the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <param name="llmInterface">The interface instance to register.</param>
        /// <returns><c>true</c> if the interface was added; otherwise, <c>false</c>.</returns>
        bool AddLLMInterface(string componentID, LLMInterface llmInterface);
        /// <summary>
        /// Adds or replaces the <see cref="STTInterface"/> for the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <param name="sttInterface">The interface instance to register.</param>
        /// <returns><c>true</c> if the interface was added; otherwise, <c>false</c>.</returns>
        bool AddSTTInterface(string componentID, STTInterface sttInterface);
        /// <summary>
        /// Adds or replaces the <see cref="TTSInterface"/> for the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <param name="ttsInterface">The interface instance to register.</param>
        /// <returns><c>true</c> if the interface was added; otherwise, <c>false</c>.</returns>
        bool AddTTSInterface(string componentID, TTSInterface ttsInterface);
        /// <summary>
        /// Adds or replaces the <see cref="TextEmbedderInterface"/> for the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <param name="textEmbedderInterface">The interface instance to register.</param>
        /// <returns><c>true</c> if the interface was added; otherwise, <c>false</c>.</returns>
        bool AddTextEmbedderInterface(string componentID, TextEmbedderInterface textEmbedderInterface);
        /// <summary>
        /// Adds or replaces the <see cref="KnowledgeInterface"/> for the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <param name="knowledgeInterface">The interface instance to register.</param>
        /// <returns><c>true</c> if the interface was added; otherwise, <c>false</c>.</returns>
        bool AddKnowledgeInterface(string componentID, KnowledgeInterface knowledgeInterface);
        /// <summary>
        /// Adds or replaces the <see cref="MCPClientInterface"/> for the specified component ID.
        /// </summary>
        /// <param name="componentID">The unique component identifier.</param>
        /// <param name="mcpClientInterface">The interface instance to register.</param>
        /// <returns><c>true</c> if the interface was added; otherwise, <c>false</c>.</returns>
        bool AddMCPClientInterface(string componentID, MCPClientInterface mcpClientInterface);
    }
    
    /// <summary>
    /// Represents the runtime context for the Inworld framework.
    /// Acts as a managed wrapper around the native context object and serves as the
    /// container for graph execution state, interface bindings, and runtime resources.
    /// </summary>
    public class InworldContext : InworldFrameworkDllClass
    {
        
    }
}