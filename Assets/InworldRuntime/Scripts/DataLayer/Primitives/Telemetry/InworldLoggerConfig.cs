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
    /// Provides configuration settings for logging within the Inworld framework.
    /// Defines log levels, output settings, and integration with external logging systems.
    /// Used for configuring how the system generates and manages log output for debugging and monitoring.
    /// </summary>
    public class InworldLoggerConfig : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldLoggerConfig class with default settings.
        /// </summary>
        public InworldLoggerConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_LoggerConfig_new(),
                InworldInterop.inworld_LoggerConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldLoggerConfig class from a native pointer.
        /// </summary>
        /// <param name="dll">The native pointer to the logger config object.</param>
        public InworldLoggerConfig(IntPtr dll)
        {
            m_DLLPtr = MemoryManager.Register(dll, InworldInterop.inworld_LoggerConfig_delete);
        }

        /// <summary>
        /// Gets or sets the minimum log level for message filtering.
        /// Determines which log messages should be processed based on their severity level.
        /// Higher values indicate more restrictive filtering (fewer messages logged).
        /// </summary>
        public int LogLevel
        {
            get => InworldInterop.inworld_LoggerConfig_log_level_get(m_DLLPtr);
            set => InworldInterop.inworld_LoggerConfig_log_level_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets whether to sink Abseil library logs into the telemetry system.
        /// When enabled, redirects Abseil logging output to the configured telemetry pipeline.
        /// </summary>
        public bool SinkAbslLogs
        {
            get => InworldInterop.inworld_LoggerConfig_sink_absl_logs_get(m_DLLPtr);
            set => InworldInterop.inworld_LoggerConfig_sink_absl_logs_set(m_DLLPtr, value);
        }
    }
}