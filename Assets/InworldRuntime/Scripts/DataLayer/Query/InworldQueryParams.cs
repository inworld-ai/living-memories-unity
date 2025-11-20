/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework
{
    /// <summary>
    /// Provides query parameters for API requests within the Inworld framework.
    /// Manages URL query parameters and request options for various Inworld services.
    /// Used for configuring logging, output formats, and other query-specific settings.
    /// </summary>
    public class InworldQueryParams : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldQueryParams class with default settings.
        /// </summary>
        public InworldQueryParams()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_QueryParams_new(),
                InworldInterop.inworld_QueryParams_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldQueryParams class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the query params object.</param>
        public InworldQueryParams(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs,
                InworldInterop.inworld_QueryParams_delete);
        }
        
        /// <summary>
        /// Gets or sets whether logging is enabled for API requests.
        /// When enabled, detailed logs of API requests and responses will be generated for debugging purposes.
        /// </summary>
        public bool IsEnableLogging
        {
            set => InworldInterop.inworld_QueryParams_enable_logging_set(m_DLLPtr, value);
            get => InworldInterop.inworld_QueryParams_enable_logging_get(m_DLLPtr);
        }
        
        /// <summary>
        /// Gets or sets the output format for API responses.
        /// Specifies the format in which response data should be returned from the API.
        /// </summary>
        /// <remarks>TODO: This should be converted to an enum for better type safety.</remarks>
        public int OutputFormat
        {
            //TODO(Yan): To Enum
            set => InworldInterop.inworld_QueryParams_output_format_set(m_DLLPtr, value);
            get => InworldInterop.inworld_QueryParams_output_format_get(m_DLLPtr);
        }
    }
}