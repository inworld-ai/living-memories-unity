/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Event;
using UnityEngine;

namespace Inworld.Framework.Knowledge
{
    /// <summary>
    /// Provides an interface for knowledge retrieval and management within the Inworld framework.
    /// Handles knowledge compilation, retrieval, and removal operations for conversational AI systems.
    /// Used for accessing and managing knowledge bases that support context-aware conversations.
    /// </summary>
    public class KnowledgeInterface : InworldInterface
    {
        /// <summary>
        /// Initializes a new instance of the KnowledgeInterface class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the knowledge interface object.</param>
        public KnowledgeInterface(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_KnowledgeInterface_delete);
        }
        
        /// <summary>
        /// Retrieves knowledge content based on the provided IDs and event history.
        /// Searches the knowledge base for relevant information given the context.
        /// </summary>
        /// <param name="ids">A vector of knowledge record IDs to retrieve.</param>
        /// <param name="eventHistory">A vector of events providing context for knowledge retrieval.</param>
        /// <returns>A vector of knowledge content strings, or null if retrieval fails.</returns>
        public InworldVector<string> GetKnowledge(InworldVector<string> ids, InworldVector<InworldEvent> eventHistory)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KnowledgeInterface_GetKnowledge(m_DLLPtr, ids.ToDLL, eventHistory.ToDLL),
                InworldInterop.inworld_StatusOr_vector_string_status,
                InworldInterop.inworld_StatusOr_vector_string_ok,
                InworldInterop.inworld_StatusOr_vector_string_value,
                InworldInterop.inworld_StatusOr_vector_string_delete
            );
            return result != IntPtr.Zero ? new InworldVector<string>(result) : null;
        }

        /// <summary>
        /// Removes a knowledge record from the knowledge base.
        /// Permanently deletes the specified knowledge entry.
        /// </summary>
        /// <param name="id">The unique identifier of the knowledge record to remove.</param>
        /// <returns>True if the knowledge was successfully removed; otherwise, false.</returns>
        public bool RemoveKnowledge(string id)
        {
            IntPtr status = InworldInterop.inworld_KnowledgeInterface_RemoveKnowledge(m_DLLPtr, id);
            bool result = InworldInterop.inworld_Status_ok(status);
            if (!result)
            {
                Debug.LogError(InworldInterop.inworld_Status_ToString(status));
            }
            return result;
        }

        /// <summary>
        /// Compiles knowledge from a collection of text records.
        /// Processes and indexes the provided text records for efficient retrieval.
        /// </summary>
        /// <param name="knowledgeID">The unique identifier for this knowledge compilation.</param>
        /// <param name="records">A vector of text records to compile into the knowledge base.</param>
        /// <returns>A vector of generated record IDs for the compiled knowledge, or null if compilation fails.</returns>
        public InworldVector<string> CompileKnowledge(string knowledgeID, InworldVector<string> records)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KnowledgeInterface_CompileKnowledge_rcstd_string_rcstd_vector_Sl_std_string_Sg_
                    (m_DLLPtr, knowledgeID, records.ToDLL),
                InworldInterop.inworld_StatusOr_vector_string_status,
                InworldInterop.inworld_StatusOr_vector_string_ok,
                InworldInterop.inworld_StatusOr_vector_string_value,
                InworldInterop.inworld_StatusOr_vector_string_delete
            );
            return result != IntPtr.Zero ? new InworldVector<string>(result) : null;
        }
        
        /// <summary>
        /// Compiles knowledge from raw file data with specified file type.
        /// Processes binary file content and creates knowledge records based on file type.
        /// </summary>
        /// <param name="knowledgeID">The unique identifier for this knowledge compilation.</param>
        /// <param name="records">A vector of character data representing the file content.</param>
        /// <param name="fileType">The integer identifier specifying the format of the file data.</param>
        /// <returns>A vector of generated record IDs for the compiled knowledge, or null if compilation fails.</returns>
        public InworldVector<string> CompileKnowledge(string knowledgeID, InworldVector<char> records, int fileType)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_KnowledgeInterface_CompileKnowledge_rcstd_string_cstd_vector_Sl_char_Sg__ceType
                    (m_DLLPtr, knowledgeID, records.ToDLL, fileType),
                InworldInterop.inworld_StatusOr_vector_string_status,
                InworldInterop.inworld_StatusOr_vector_string_ok,
                InworldInterop.inworld_StatusOr_vector_string_value,
                InworldInterop.inworld_StatusOr_vector_string_delete
            );
            return result != IntPtr.Zero ? new InworldVector<string>(result) : null;
        }
    }
}