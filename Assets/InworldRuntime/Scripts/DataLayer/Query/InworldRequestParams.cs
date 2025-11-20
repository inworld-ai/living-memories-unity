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
    /// Represents request parameters for Inworld API calls.
    /// This class manages configuration settings that are sent with API requests.
    /// </summary>
    public class InworldRequestParams : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldRequestParams class with default settings.
        /// </summary>
        public InworldRequestParams()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_RequestParams_new(),
                InworldInterop.inworld_RequestParams_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldRequestParams class from an existing native pointer.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing request parameters instance.</param>
        public InworldRequestParams(IntPtr rhs)
        {            
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_RequestParams_delete);
        }
        
        /// <summary>
        /// Gets or sets the model identifier to use for API requests.
        /// This specifies which AI model should process the request.
        /// </summary>
        /// <value>The model ID as a string.</value>
        public string ModelID
        {
            set => InworldInterop.inworld_RequestParams_model_id_set(m_DLLPtr, value);
            get => InworldInterop.inworld_RequestParams_model_id_get(m_DLLPtr);
        }
        
        /// <summary>
        /// Gets or sets the text normalization setting for requests.
        /// This controls how text is processed and normalized before being sent to the API.
        /// </summary>
        /// <value>An integer value representing the text normalization mode.</value>
        /// <remarks>TODO: This should be converted to an enum for better type safety.</remarks>
        public int TextNormalization
        {
            //TODO(Yan): To Enum
            set => InworldInterop.inworld_RequestParams_apply_text_normalization_set(m_DLLPtr, value);
            get => InworldInterop.inworld_RequestParams_apply_text_normalization_get(m_DLLPtr);
        }
    }
}