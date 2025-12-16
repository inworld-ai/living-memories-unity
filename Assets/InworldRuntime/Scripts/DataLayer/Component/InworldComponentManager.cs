/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

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
    /// <summary>
    /// Static manager class for registering, creating, and managing AI component interfaces within the Inworld framework.
    /// Provides factory methods for creating various AI service interfaces (LLM, STT, TTS, etc.) and manages component registration.
    /// Handles the lifecycle of AI components and provides utilities for custom node and edge condition registration.
    /// </summary>
    public static class InworldComponentManager
    {
        /// <summary>
        /// Checks whether a component with the specified name is currently registered in the component registry.
        /// </summary>
        /// <param name="componentName">The name of the component to check for registration.</param>
        /// <returns>True if the component is registered; otherwise, false.</returns>
        public static bool IsRegistered(string componentName)
        {
            return InworldInterop.inworld_ComponentRegistry_IsComponentRegistered(InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName);
        }

        /// <summary>
        /// Unregisters a component with the specified name from the component registry.
        /// This removes the component from the registry, making it unavailable for future operations.
        /// </summary>
        /// <param name="componentName">The name of the component to unregister.</param>
        public static void Unregister(string componentName)
        {
            InworldInterop.inworld_ComponentRegistry_UnregisterComponent(InworldInterop.inworld_ComponentRegistry_GetInstance(), componentName);
        }

        /// <summary>
        /// Clears all registered components from the component registry.
        /// This removes all components and resets the registry to an empty state.
        /// </summary>
        public static void Clear()
        {
            InworldInterop.inworld_ComponentRegistry_Clear(InworldInterop.inworld_ComponentRegistry_GetInstance());
        }

        /// <summary>
        /// Creates a new LLM interface using local configuration.
        /// This method creates an interface for running language models locally on the device.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this LLM component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="localConfig">The local configuration specifying model path, device, and other local settings.</param>
        /// <returns>A new LLMInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new LLM interface using remote configuration.
        /// This method creates an interface for connecting to cloud-based language model services.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this LLM component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="remoteConfig">The remote configuration specifying API keys and service endpoints.</param>
        /// <returns>A new LLMInterface instance if successful; null if creation failed.</returns>
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

        /// <summary>
        /// Creates a new LLM interface using routing configuration.
        /// This method creates an interface that can route requests to multiple LLM providers based on configuration.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this LLM component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="config">The routing configuration specifying how to distribute requests across multiple providers.</param>
        /// <returns>A new LLMInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new STT (Speech-to-Text) interface using local configuration.
        /// This method creates an interface for running speech recognition models locally on the device.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this STT component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="localConfig">The local configuration specifying model path, device, and other local settings.</param>
        /// <returns>A new STTInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new STT (Speech-to-Text) interface using remote configuration.
        /// This method creates an interface for connecting to cloud-based speech recognition services.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this STT component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="remoteConfig">The remote configuration specifying API keys and service endpoints.</param>
        /// <returns>A new STTInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new TTS (Text-to-Speech) interface using remote configuration.
        /// This method creates an interface for connecting to cloud-based speech synthesis services.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this TTS component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="remoteConfig">The remote configuration specifying API keys and service endpoints.</param>
        /// <returns>A new TTSInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new Text Embedder interface using local configuration.
        /// This method creates an interface for running text embedding models locally on the device.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this Text Embedder component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="localConfig">The local configuration specifying model path, device, and other local settings.</param>
        /// <returns>A new TextEmbedderInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new Text Embedder interface using remote configuration.
        /// This method creates an interface for connecting to cloud-based text embedding services.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this Text Embedder component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="remoteConfig">The remote configuration specifying API keys and service endpoints.</param>
        /// <returns>A new TextEmbedderInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new Knowledge interface using local configuration.
        /// This method creates an interface for running knowledge retrieval models locally on the device.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this Knowledge component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="localConfig">The local configuration specifying model path, device, and other local settings.</param>
        /// <returns>A new KnowledgeInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new Knowledge interface using remote configuration.
        /// This method creates an interface for connecting to cloud-based knowledge retrieval services.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this Knowledge component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="remoteConfig">The remote configuration specifying API keys and service endpoints.</param>
        /// <returns>A new KnowledgeInterface instance if successful; null if creation failed.</returns>
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
        
        /// <summary>
        /// Creates a new MCP (Model Context Protocol) Client interface.
        /// This method creates an interface for connecting to MCP-compatible services and tools.
        /// </summary>
        /// <param name="componentName">The unique name identifier for this MCP Client component.</param>
        /// <param name="context">The creation context containing environment and configuration data.</param>
        /// <param name="config">The MCP client configuration specifying connection settings and capabilities.</param>
        /// <returns>A new MCPClientInterface instance if successful; null if creation failed.</returns>
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

        /// <summary>
        /// Registers a custom edge condition executor with the component registry.
        /// This allows custom logic to be executed when evaluating graph edge conditions.
        /// </summary>
        /// <param name="edgeConditionName">The unique name for this custom edge condition.</param>
        /// <param name="executor">The executor that will handle the edge condition evaluation logic.</param>
        /// <returns>True if registration was successful; false if registration failed.</returns>
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
        
        /// <summary>
        /// Registers a custom threaded edge condition executor with the component registry.
        /// This allows custom logic to be executed in a separate thread when evaluating graph edge conditions.
        /// </summary>
        /// <param name="edgeConditionName">The unique name for this custom edge condition.</param>
        /// <param name="executor">The threaded executor that will handle the edge condition evaluation logic.</param>
        /// <returns>True if registration was successful; false if registration failed.</returns>
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

        /// <summary>
        /// Registers a custom node creator with the component registry.
        /// This allows custom node types to be created and used in graph workflows.
        /// </summary>
        /// <param name="nodeName">The unique name for this custom node type.</param>
        /// <param name="executor">The threaded executor that will handle the custom node creation logic.</param>
        /// <returns>True if registration was successful; false if registration failed.</returns>
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