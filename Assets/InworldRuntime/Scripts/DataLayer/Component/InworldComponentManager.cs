using System;
using Inworld.Framework.Edge;
using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Node;
using Inworld.Framework.STT;
using Inworld.Framework.TextEmbedder;
using Inworld.Framework.TTS;
using UnityEngine;

namespace Inworld.Framework
{
    public static class InworldComponentManager
    {
        public static bool IsRegistered(string componentName)
        {
            return InworldInterop.inworld_ComponentRegistry_IsComponentRegistered(InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName);
        }

        public static void Unregister(string componentName)
        {
            InworldInterop.inworld_ComponentRegistry_UnregisterComponent(InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName);
        }

        public static void Clear()
        {
            InworldInterop.inworld_ComponentRegistry_Clear(InworldInterop.inworld_ComponentRegistry_GetInstance());
        }

        public static LLMInterface CreateLLMInterface(string componentName, InworldCreationContext context,
            LLMLocalConfig localConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_LLMInterface_rcstd_string_rcinworld_CreationContext_rcinworld_LocalLLMConfig(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        localConfig.ToDLL),
                InworldInterop.inworld_StatusOr_LLMInterface_status,
                InworldInterop.inworld_StatusOr_LLMInterface_ok,
                InworldInterop.inworld_StatusOr_LLMInterface_value,
                InworldInterop.inworld_StatusOr_LLMInterface_delete
            );
            return result == IntPtr.Zero ? null : new LLMInterface(result);
        }
        
        public static LLMInterface CreateLLMInterface(string componentName, InworldCreationContext context,
            LLMRemoteConfig remoteConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_LLMInterface_rcstd_string_rcinworld_CreationContext_rcinworld_RemoteLLMConfig(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        remoteConfig.ToDLL),
                InworldInterop.inworld_StatusOr_LLMInterface_status,
                InworldInterop.inworld_StatusOr_LLMInterface_ok,
                InworldInterop.inworld_StatusOr_LLMInterface_value,
                InworldInterop.inworld_StatusOr_LLMInterface_delete
            );
            return result == IntPtr.Zero ? null : new LLMInterface(result);
        }

        public static LLMInterface CreateLLMInterface(string componentName, InworldCreationContext context,
            LLMRoutingCreationConfig config)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_LLMRouting(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        config.ToDLL),
                InworldInterop.inworld_StatusOr_LLMInterface_status,
                InworldInterop.inworld_StatusOr_LLMInterface_ok,
                InworldInterop.inworld_StatusOr_LLMInterface_value,
                InworldInterop.inworld_StatusOr_LLMInterface_delete
            );
            return result == IntPtr.Zero ? null : new LLMInterface(result);
        }
        
        public static STTInterface CreateSTTInterface(string componentName, InworldCreationContext context,
            STTLocalConfig localConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_STTInterface_rcstd_string_rcinworld_CreationContext_rcinworld_LocalSTTConfig(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        localConfig.ToDLL),
                InworldInterop.inworld_StatusOr_STTInterface_status,
                InworldInterop.inworld_StatusOr_STTInterface_ok,
                InworldInterop.inworld_StatusOr_STTInterface_value,
                InworldInterop.inworld_StatusOr_STTInterface_delete
            );
            return result == IntPtr.Zero ? null : new STTInterface(result);
        }
        
        public static STTInterface CreateSTTInterface(string componentName, InworldCreationContext context,
            STTRemoteConfig remoteConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_STTInterface_rcstd_string_rcinworld_CreationContext_rcinworld_RemoteSTTConfig(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        remoteConfig.ToDLL),
                InworldInterop.inworld_StatusOr_STTInterface_status,
                InworldInterop.inworld_StatusOr_STTInterface_ok,
                InworldInterop.inworld_StatusOr_STTInterface_value,
                InworldInterop.inworld_StatusOr_STTInterface_delete
            );
            return result == IntPtr.Zero ? null : new STTInterface(result);
        }
        
        public static TTSInterface CreateTTSInterface(string componentName, InworldCreationContext context,
            TTSRemoteConfig remoteConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_TTSInterface(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        remoteConfig.ToDLL),
                InworldInterop.inworld_StatusOr_TTSInterface_status,
                InworldInterop.inworld_StatusOr_TTSInterface_ok,
                InworldInterop.inworld_StatusOr_TTSInterface_value,
                InworldInterop.inworld_StatusOr_TTSInterface_delete
            );
            return result == IntPtr.Zero ? null : new TTSInterface(result);
        }
        
        public static TextEmbedderInterface CreateTextEmbedderInterface(string componentName, InworldCreationContext context,
            TextEmbedderLocalConfig localConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_TextEmbedderInterface_rcstd_string_rcinworld_CreationContext_rcinworld_LocalTextEmbedderConfig(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        localConfig.ToDLL),
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_status,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_ok,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_value,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_delete
            );
            return result == IntPtr.Zero ? null : new TextEmbedderInterface(result);
        }
        
        public static TextEmbedderInterface CreateTextEmbedderInterface(string componentName, InworldCreationContext context,
            TextEmbedderRemoteConfig remoteConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_TextEmbedderInterface_rcstd_string_rcinworld_CreationContext_rcinworld_RemoteTextEmbedderConfig(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        remoteConfig.ToDLL),
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_status,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_ok,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_value,
                InworldInterop.inworld_StatusOr_TextEmbedderInterface_delete
            );
            return result == IntPtr.Zero ? null : new TextEmbedderInterface(result);
        }
        
        public static KnowledgeInterface CreateKnowledgeInterface(string componentName, InworldCreationContext context,
            KnowledgeLocalConfig localConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_KnowledgeInterface_rcstd_string_rcinworld_CreationContext_rcinworld_LocalKnowledgeConfig(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        localConfig.ToDLL),
                InworldInterop.inworld_StatusOr_KnowledgeInterface_status,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_ok,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_value,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_delete
            );
            return result == IntPtr.Zero ? null : new KnowledgeInterface(result);
        }
        
        public static KnowledgeInterface CreateKnowledgeInterface(string componentName, InworldCreationContext context,
            KnowledgeRemoteConfig remoteConfig)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_KnowledgeInterface_rcstd_string_rcinworld_CreationContext_rcinworld_RemoteKnowledgeConfig(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        remoteConfig.ToDLL),
                InworldInterop.inworld_StatusOr_KnowledgeInterface_status,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_ok,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_value,
                InworldInterop.inworld_StatusOr_KnowledgeInterface_delete
            );
            return result == IntPtr.Zero ? null : new KnowledgeInterface(result);
        }
        
        public static MCPClientInterface CreateMCPClientInterface(string componentName, InworldCreationContext context,
            MCPClientCreationConfig config)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop
                    .inworld_ComponentRegistry_CreateComponent_MCPClientInterface(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName, context.ToDLL,
                        config.ToDLL),
                InworldInterop.inworld_StatusOr_MCPClientInterface_status,
                InworldInterop.inworld_StatusOr_MCPClientInterface_ok,
                InworldInterop.inworld_StatusOr_MCPClientInterface_value,
                InworldInterop.inworld_StatusOr_MCPClientInterface_delete
            );
            return result == IntPtr.Zero ? null : new MCPClientInterface(result);
        }

        public static bool RegisterCustomEdgeCondition(string edgeConditionName, EdgeConditionExecutor executor)
        {
            IntPtr result =
                InworldInterop
                    .inworld_ComponentRegistry_RegisterCustomEdgeCondition_rcstd_string_rcinworld_swig_helpers_EdgeConditionExecutor(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), edgeConditionName, executor.ToDLL);

            if (InworldInterop.inworld_Status_ok(result)) 
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(result));
            return false;
        }
        
        public static bool RegisterCustomEdgeCondition(string edgeConditionName, EdgeConditionThreadedExecutor executor)
        {
            IntPtr result =
                InworldInterop
                    .inworld_ComponentRegistry_RegisterCustomEdgeCondition_rcstd_string_rcinworld_swig_helpers_EdgeConditionThreadedExecutor(
                        InworldInterop.inworld_ComponentRegistry_GetInstance(), edgeConditionName, executor.ToDLL);

            if (InworldInterop.inworld_Status_ok(result)) 
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(result));
            return false;
        }

        public static bool RegisterCustomNode(string nodeName, CustomNodeThreadedCreateExecutor executor)
        {
            IntPtr result =
                InworldInterop.inworld_ComponentRegistry_RegisterCustomNode(
                    InworldInterop.inworld_ComponentRegistry_GetInstance(), nodeName, executor.ToDLL);
            
            if (InworldInterop.inworld_Status_ok(result)) 
                return true;
            Debug.LogError(InworldInterop.inworld_Status_ToString(result));
            return false;
        }
        
        // YAN: Unable to use RegisterEdgeConditionCallback and RegisterCallbackNode as their required input parameter is ffi callback from nodejs.
        //      In order to use, we need to create the equivalent C API bindings.
    }
}