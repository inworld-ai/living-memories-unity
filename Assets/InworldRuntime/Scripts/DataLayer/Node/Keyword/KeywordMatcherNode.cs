using System;

namespace Inworld.Framework.Node
{
    public class KeywordMatcherNode : InworldNode
    {
        public KeywordMatcherNode(string nodeName, KeywordMatcherNodeCreationConfig creationConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KeywordMatcherNode_Create_rcstd_string_rcinworld_graphs_KeywordMatcherNodeCreationConfig
                    (nodeName, creationConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_KeywordMatcherNode_delete);
        }
        
        public KeywordMatcherNode(string nodeName, KeywordMatcherNodeCreationConfig creationConfig, NodeExecutionConfig executionConfig)
        {
            IntPtr genResult = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KeywordMatcherNode_Create_rcstd_string_rcinworld_graphs_KeywordMatcherNodeCreationConfig_rcstd_shared_ptr_Sl_inworld_graphs_NodeExecutionConfig_Sg_
                    (nodeName, creationConfig.ToDLL, executionConfig.ToDLL),
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_status,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_value,
                InworldInterop.inworld_StatusOr_SharedPtr_KeywordMatcherNode_delete);
            if (genResult != IntPtr.Zero)
                m_DLLPtr = MemoryManager.Register(genResult, InworldInterop.inworld_KeywordMatcherNode_delete);
        }
        
        public KeywordMatcherNode(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_KeywordMatcherNode_delete);
        }

        public override string ID => InworldInterop.inworld_KeywordMatcherNode_id(m_DLLPtr);

        public override bool IsValid => InworldInterop.inworld_KeywordMatcherNode_is_valid(m_DLLPtr);

        public MatchedKeywords Process(ProcessContext processContext, InworldText text)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KeywordMatcherNode_Process(m_DLLPtr, processContext.ToDLL, text.ToDLL), 
                InworldInterop.inworld_StatusOr_SharedPtr_MatchedKeywords_status,
                InworldInterop.inworld_StatusOr_SharedPtr_MatchedKeywords_ok,
                InworldInterop.inworld_StatusOr_SharedPtr_MatchedKeywords_value,
                InworldInterop.inworld_StatusOr_SharedPtr_MatchedKeywords_delete
            );
            return result != IntPtr.Zero ? new MatchedKeywords(result) : null;
        }
    }
}