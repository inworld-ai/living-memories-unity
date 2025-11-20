using System;
using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Node;
using Inworld.Framework.STT;
using Inworld.Framework.TextEmbedder;
using Inworld.Framework.TTS;
using UnityEngine;

namespace Inworld.Framework
{
    public class ComponentStore : InworldContext, IGetNodeHandler, IAddInterfaceHandler
    {
        public ComponentStore()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ComponentStore_new(), InworldInterop.inworld_ComponentStore_delete);
        }

        public ComponentStore(string variant)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ComponentStore_new_rcstd_string(variant), InworldInterop.inworld_ComponentStore_delete);

        }
        public ComponentStore(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ComponentStore_delete);
        }

        public void Clear() => InworldInterop.inworld_ComponentStore_Clear(m_DLLPtr);

        public GoalAdvancementNode GetGoalAdvancementNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_GoalAdvancementNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_GoalAdvancementNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_GoalAdvancementNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_GoalAdvancementNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_GoalAdvancementNode_delete
            );
            return result == IntPtr.Zero ? null : new GoalAdvancementNode(result); 
        }
        
        public IntentNode GetIntentNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_IntentNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_IntentNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_IntentNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_IntentNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_IntentNode_delete
            );
            return result == IntPtr.Zero ? null : new IntentNode(result); 
        }
        
        public KeywordMatcherNode GetKeywordMatcherNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_KeywordMatcherNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_delete
            );
            return result == IntPtr.Zero ? null : new KeywordMatcherNode(result); 
        }
        
        public KnowledgeNode GetKnowledgeNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_KnowledgeNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_KnowledgeNode_delete
            );
            return result == IntPtr.Zero ? null : new KnowledgeNode(result); 
        }
        
        public LLMChatNode GetLLMChatNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_LLMChatNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatNode_delete
            );
            return result == IntPtr.Zero ? null : new LLMChatNode(result); 
        }
        
        public LLMChatRequestBuilderNode GetLLMChatRequestBuilderNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_LLMChatRequestBuilderNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequestBuilderNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequestBuilderNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequestBuilderNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMChatRequestBuilderNode_delete
            );
            return result == IntPtr.Zero ? null : new LLMChatRequestBuilderNode(result); 
        }
        
        public LLMCompletionNode GetLLMCompletionNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_LLMCompletionNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMCompletionNode_delete
            );
            return result == IntPtr.Zero ? null : new LLMCompletionNode(result); 
        }
        
        public LLMPromptBuilderNode GetLLMPromptBuilderNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_LLMPromptBuilderNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_LLMPromptBuilderNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMPromptBuilderNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMPromptBuilderNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_LLMPromptBuilderNode_delete
            );
            return result == IntPtr.Zero ? null : new LLMPromptBuilderNode(result); 
        }
        
        public MemoryUpdateNode GetMemoryUpdateNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_MemoryUpdateNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryUpdateNode_delete
            );
            return result == IntPtr.Zero ? null : new MemoryUpdateNode(result); 
        }
        
        public MemoryRetrieveNode GetMemoryRetrieveNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_MemoryRetrieveNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryRetrieveNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryRetrieveNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryRetrieveNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MemoryRetrieveNode_delete
            );
            return result == IntPtr.Zero ? null : new MemoryRetrieveNode(result); 
        }
        
        public RandomCannedTextNode GetRandomCannedTextNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_RandomCannedTextNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_RandomCannedTextNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_RandomCannedTextNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_RandomCannedTextNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_RandomCannedTextNode_delete
            );
            return result == IntPtr.Zero ? null : new RandomCannedTextNode(result); 
        }
        
        public SafetyCheckerNode GetSafetyCheckerNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_SafetyCheckerNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyCheckerNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyCheckerNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyCheckerNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_SafetyCheckerNode_delete
            );
            return result == IntPtr.Zero ? null : new SafetyCheckerNode(result); 
        }
        
        public STTNode GetSTTNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_STTNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_STTNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_STTNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_STTNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_STTNode_delete
            );
            return result == IntPtr.Zero ? null : new STTNode(result); 
        }
        
        public TextAggregatorNode GetTextAggregatorNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_TextAggregatorNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_TextAggregatorNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_TextAggregatorNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_TextAggregatorNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_TextAggregatorNode_delete
            );
            return result == IntPtr.Zero ? null : new TextAggregatorNode(result); 
        }
        
        public TextChunkingNode GetTextChunkingNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_TextChunkingNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_TextChunkingNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_TextChunkingNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_TextChunkingNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_TextChunkingNode_delete
            );
            return result == IntPtr.Zero ? null : new TextChunkingNode(result); 
        }
        
        public TTSNode GetTTSNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_TTSNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_TTSNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_TTSNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_TTSNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_TTSNode_delete
            );
            return result == IntPtr.Zero ? null : new TTSNode(result); 
        }
        
        public MCPListToolsNode GetMCPListToolsNode(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_MCPListToolsNode(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_SharedPtr_MCPListToolsNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPListToolsNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPListToolsNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MCPListToolsNode_delete
            );
            return result == IntPtr.Zero ? null : new MCPListToolsNode(result); 
        }
        
        public LLMInterface GetLLMInterface(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_LLMInterface(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_LLMInterface_status,
                InworldInterop.inworld_StatusOr_LLMInterface_ok,
                InworldInterop.inworld_StatusOr_LLMInterface_value,
                InworldInterop.inworld_StatusOr_LLMInterface_delete
            );
            return result == IntPtr.Zero ? null : new LLMInterface(result); 
        }
        
        public STTInterface GetSTTInterface(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_STTInterface(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_STTInterface_status,
                InworldInterop.inworld_StatusOr_STTInterface_ok,
                InworldInterop.inworld_StatusOr_STTInterface_value,
                InworldInterop.inworld_StatusOr_STTInterface_delete
            );
            return result == IntPtr.Zero ? null : new STTInterface(result); 
        }
        
        public TTSInterface GetTTSInterface(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_TTSInterface(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_TTSInterface_status,
                InworldInterop.inworld_StatusOr_TTSInterface_ok,
                InworldInterop.inworld_StatusOr_TTSInterface_value,
                InworldInterop.inworld_StatusOr_TTSInterface_delete
            );
            return result == IntPtr.Zero ? null : new TTSInterface(result); 
        }
        
        public TextEmbedderInterface GetTextEmbedderInterface(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_TextEmbedderInterface(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_status,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_ok,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_value,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_delete
            );
            return result == IntPtr.Zero ? null : new TextEmbedderInterface(result); 
        }
        
        public KnowledgeInterface GetKnowledgeInterface(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_KnowledgeInterface(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_KnowledgeInterface_status,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_ok,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_value,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_delete
            );
            return result == IntPtr.Zero ? null : new KnowledgeInterface(result); 
        }
        
        public MCPClientInterface GetMCPClientInterface(string componentID)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_ComponentStore_GetComponent_MCPClientInterface(m_DLLPtr, componentID),
                InworldInterop.inworld_StatusOr_MCPClientInterface_status,
                InworldInterop.inworld_StatusOr_MCPClientInterface_ok,
                InworldInterop.inworld_StatusOr_MCPClientInterface_value,
                InworldInterop.inworld_StatusOr_MCPClientInterface_delete
            );
            return result == IntPtr.Zero ? null : new MCPClientInterface(result); 
        }

        public bool AddLLMInterface(string componentID, LLMInterface llmInterface)
        {
            IntPtr status = InworldInterop.inworld_ComponentStore_AddComponent_LLMInterface(m_DLLPtr, componentID, llmInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddSTTInterface(string componentID, STTInterface sttInterface)
        {
            IntPtr status = InworldInterop.inworld_ComponentStore_AddComponent_STTInterface(m_DLLPtr, componentID, sttInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddTTSInterface(string componentID, TTSInterface ttsInterface)
        {
            IntPtr status = InworldInterop.inworld_ComponentStore_AddComponent_TTSInterface(m_DLLPtr, componentID, ttsInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddTextEmbedderInterface(string componentID, TextEmbedderInterface textEmbedderInterface)
        {
            IntPtr status = InworldInterop.inworld_ComponentStore_AddComponent_TextEmbedderInterface(m_DLLPtr, componentID, textEmbedderInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddKnowledgeInterface(string componentID, KnowledgeInterface knowledgeInterface)
        {
            IntPtr status = InworldInterop.inworld_ComponentStore_AddComponent_KnowledgeInterface(m_DLLPtr, componentID, knowledgeInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
        
        public bool AddMCPClientInterface(string componentID, MCPClientInterface mcpClientInterface)
        {
            IntPtr status = InworldInterop.inworld_ComponentStore_AddComponent_MCPClientInterface(m_DLLPtr, componentID, mcpClientInterface.ToDLL);
            if (InworldInterop.inworld_Status_ok(status))
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            return false;
        }
    }
}