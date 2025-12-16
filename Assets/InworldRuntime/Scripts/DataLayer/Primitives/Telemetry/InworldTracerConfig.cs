/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Telemetry
{
    /// <summary>
    /// Provides configuration settings for distributed tracing within the Inworld framework.
    /// Defines tracing parameters such as sampling rates for performance monitoring.
    /// Used for configuring how distributed traces are collected and sampled across system operations.
    /// </summary>
    public class InworldTracerConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the InworldTracerConfig class with default settings.
        /// </summary>
        public InworldTracerConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TracerConfig_new(), InworldInterop.inworld_TracerConfig_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldTracerConfig class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the tracer config object.</param>
        public InworldTracerConfig(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_TracerConfig_delete);
        }

        /// <summary>
        /// Gets or sets the sampling rate for distributed tracing.
        /// Determines what percentage of traces should be collected for analysis.
        /// Values range from 0.0 (no sampling) to 1.0 (sample all traces).
        /// </summary>
        public double SampleRate
        {
            get => InworldInterop.inworld_TracerConfig_sampling_rate_get(m_DLLPtr);
            set => InworldInterop.inworld_TracerConfig_sampling_rate_set(m_DLLPtr, value);
        }
    }
}