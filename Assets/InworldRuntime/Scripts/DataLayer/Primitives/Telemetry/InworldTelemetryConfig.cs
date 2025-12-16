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
    /// Provides configuration settings for telemetry services within the Inworld framework.
    /// Manages telemetry data collection, logging, tracing, and export configuration.
    /// Used for configuring observability and monitoring capabilities for the Inworld system.
    /// </summary>
    public class InworldTelemetryConfig : InworldConfig
    {
        /// <summary>
        /// Initializes a new instance of the InworldTelemetryConfig class with default settings.
        /// </summary>
        public InworldTelemetryConfig()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TelemetryConfig_new(),
                InworldInterop.inworld_TelemetryConfig_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldTelemetryConfig class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the telemetry config object.</param>
        public InworldTelemetryConfig(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TelemetryConfig_delete);
        }

        /// <summary>
        /// Gets or sets the endpoint URL for telemetry data export.
        /// Specifies where telemetry data should be sent for collection and analysis.
        /// </summary>
        public string EndPoint
        {
            get => InworldInterop.inworld_TelemetryConfig_endpoint_get(m_DLLPtr);
            set => InworldInterop.inworld_TelemetryConfig_endpoint_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the type of telemetry exporter to use.
        /// Determines the format and protocol for exporting telemetry data.
        /// </summary>
        public int ExporterType
        {
            get => InworldInterop.inworld_TelemetryConfig_exporter_type_get(m_DLLPtr);
            set => InworldInterop.inworld_TelemetryConfig_exporter_type_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the logger configuration for telemetry.
        /// Defines how telemetry-related logging should be handled and configured.
        /// </summary>
        public InworldLoggerConfig Logger
        {
            get => new(InworldInterop.inworld_TelemetryConfig_logger_get(m_DLLPtr));
            set => InworldInterop.inworld_TelemetryConfig_logger_set(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Gets or sets the API key for authenticating with telemetry services.
        /// Provides authentication credentials for accessing telemetry collection endpoints.
        /// </summary>
        public string APIKey
        {
            get => InworldInterop.inworld_TelemetryConfig_api_key_get(m_DLLPtr);
            set => InworldInterop.inworld_TelemetryConfig_api_key_set(m_DLLPtr, value);
        }
        
        /// <summary>
        /// Gets or sets the name of the service for telemetry identification.
        /// Used to identify the source service in telemetry data for monitoring and analysis.
        /// </summary>
        public string ServiceName
        {
            get => InworldInterop.inworld_TelemetryConfig_service_name_get(m_DLLPtr);
            set => InworldInterop.inworld_TelemetryConfig_service_name_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the version of the service for telemetry tracking.
        /// Used to track telemetry data across different versions of the service.
        /// </summary>
        public string ServiceVersion
        {
            get => InworldInterop.inworld_TelemetryConfig_service_version_get(m_DLLPtr);
            set => InworldInterop.inworld_TelemetryConfig_service_version_set(m_DLLPtr, value);
        }

        /// <summary>
        /// Gets or sets the tracer configuration for distributed tracing.
        /// Defines how distributed tracing spans should be configured and sampled.
        /// </summary>
        public InworldTracerConfig Tracer
        {
            get => new InworldTracerConfig(InworldInterop.inworld_TelemetryConfig_tracer_get(m_DLLPtr));
            set => InworldInterop.inworld_TelemetryConfig_tracer_set(m_DLLPtr, value.ToDLL);
        }

        /// <summary>
        /// Applies this telemetry configuration to the system.
        /// Configures the telemetry system with the current settings for data collection and export.
        /// </summary>
        public void Config()
        {
            InworldInterop.inworld_ConfigureTelemetry_rcinworld_TelemetryConfig(m_DLLPtr);
        }
    }
}