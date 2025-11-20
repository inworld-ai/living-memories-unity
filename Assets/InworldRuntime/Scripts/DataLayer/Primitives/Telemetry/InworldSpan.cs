/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;

namespace Inworld.Framework.Telemetry
{
    /// <summary>
    /// Represents a distributed tracing span within the Inworld framework.
    /// Provides functionality to track and measure the duration and context of operations.
    /// Used for performance monitoring, debugging, and understanding system behavior across service boundaries.
    /// </summary>
    public class InworldSpan : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldSpan class with the specified span name.
        /// Creates and starts a new tracing span for measuring operation duration.
        /// </summary>
        /// <param name="spanName">The name of the span for identification in traces.</param>
        public InworldSpan(string spanName)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_StartSpan_rcstd_string(spanName),
                InworldInterop.inworld_Span_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldSpan class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the span object.</param>
        public InworldSpan(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Span_delete);
        }

        /// <summary>
        /// Ends the span and records its completion time.
        /// Should be called when the tracked operation is finished to capture accurate timing data.
        /// </summary>
        public void End() => InworldInterop.inworld_Span_End(m_DLLPtr);

        /// <summary>
        /// Sets a string attribute on the span for additional context.
        /// Adds metadata to the span that can be used for filtering and analysis in tracing systems.
        /// </summary>
        /// <param name="key">The attribute key name.</param>
        /// <param name="value">The attribute value.</param>
        public void SetAttribute(string key, string value)
        {
            InworldInterop.inworld_Span_SetAttribute_rcstd_string_rcstd_string(m_DLLPtr, key, value);
        }

        /// <summary>
        /// Sets a list of string values as an attribute on the span.
        /// Adds array-type metadata to the span for complex contextual information.
        /// </summary>
        /// <param name="key">The attribute key name.</param>
        /// <param name="values">The list of attribute values.</param>
        public void SetAttribute(string key, List<string> values)
        {
            InworldVector<string> inputValues = new InworldVector<string>();
            inputValues.AddRange(values);
            InworldInterop.inworld_Span_SetAttribute_rcstd_string_rcstd_vector_Sl_std_string_Sg_(m_DLLPtr, key, inputValues.ToDLL);
        }

        /// <summary>
        /// Marks the span as successful with an OK status.
        /// Indicates that the tracked operation completed successfully without errors.
        /// </summary>
        public void SetOK()
        {
            InworldInterop.inworld_Span_SetOK(m_DLLPtr);
        }

        /// <summary>
        /// Marks the span as failed with an error status and message.
        /// Records error information for the tracked operation for debugging and monitoring.
        /// </summary>
        /// <param name="error">The error message describing what went wrong.</param>
        public void SetError(string error)
        {
            InworldInterop.inworld_Span_SetError(m_DLLPtr, error);
        }
    }
}