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
    /// Builder class for creating telemetry configuration objects within the Inworld framework.
    /// Provides a fluent interface for constructing telemetry configurations with various parameters.
    /// Used for building complete telemetry configurations from individual settings or data objects.
    /// </summary>
    public class InworldTelemetryBuilder : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldTelemetryBuilder class with default settings.
        /// </summary>
        public InworldTelemetryBuilder()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_TelemetryConfigBuilder_new(),
                InworldInterop.inworld_TelemetryConfigBuilder_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldTelemetryBuilder class from a native pointer.
        /// </summary>
        /// <param name="rhs">The native pointer to the telemetry builder object.</param>
        public InworldTelemetryBuilder(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_TelemetryConfigBuilder_delete);
        }

        /// <summary>
        /// Builds a telemetry configuration from an InworldTelemetry data object.
        /// Creates a complete telemetry configuration using all settings from the provided data object.
        /// </summary>
        /// <param name="telemetryConfig">The telemetry data object containing all configuration settings.</param>
        /// <returns>A fully configured InworldTelemetryConfig instance.</returns>
        public InworldTelemetryConfig Build(InworldTelemetry telemetryConfig)
        {
            InworldInterop.inworld_TelemetryConfigBuilder_SetServiceName(m_DLLPtr, telemetryConfig.serviceName);
            InworldInterop.inworld_TelemetryConfigBuilder_SetServiceVersion(m_DLLPtr, telemetryConfig.serviceVersion);
            InworldInterop.inworld_TelemetryConfigBuilder_SetEndpoint(m_DLLPtr, telemetryConfig.endPoint);
            InworldInterop.inworld_TelemetryConfigBuilder_SetExporterType(m_DLLPtr, telemetryConfig.exporterType);
            InworldInterop.inworld_TelemetryConfigBuilder_SetLogLevel(m_DLLPtr, telemetryConfig.logLevel);
            InworldInterop.inworld_TelemetryConfigBuilder_SetSamplingRate(m_DLLPtr, telemetryConfig.samplingRate);
            InworldInterop.inworld_TelemetryConfigBuilder_SetSinkAbslLogs(m_DLLPtr, telemetryConfig.sinkAbslLogs);
            InworldInterop.inworld_TelemetryConfigBuilder_SetApiKey(m_DLLPtr, telemetryConfig.telemetryKey);
            return new InworldTelemetryConfig(InworldInterop.inworld_TelemetryConfigBuilder_Build(m_DLLPtr));
        }
        
        /// <summary>
        /// Builds a telemetry configuration from individual parameter values.
        /// Creates a telemetry configuration using explicitly provided parameters.
        /// </summary>
        /// <param name="serviceName">The name of the service for telemetry identification.</param>
        /// <param name="serviceVersion">The version of the service for telemetry tracking.</param>
        /// <param name="endPoint">The endpoint URL for telemetry data export.</param>
        /// <param name="exporterType">The type of telemetry exporter to use.</param>
        /// <param name="logLevel">The minimum log level for message filtering.</param>
        /// <param name="samplingRate">The sampling rate for distributed tracing.</param>
        /// <param name="sinkAbslLogs">Whether to sink Abseil library logs into the telemetry system.</param>
        /// <returns>A fully configured InworldTelemetryConfig instance.</returns>
        public InworldTelemetryConfig Build(
            string serviceName, 
            string serviceVersion, 
            string endPoint, 
            int exporterType, 
            int logLevel, 
            double samplingRate, 
            bool sinkAbslLogs)
        {
            InworldInterop.inworld_TelemetryConfigBuilder_SetServiceName(m_DLLPtr, serviceName);
            InworldInterop.inworld_TelemetryConfigBuilder_SetServiceVersion(m_DLLPtr, serviceVersion);
            InworldInterop.inworld_TelemetryConfigBuilder_SetEndpoint(m_DLLPtr, endPoint);
            InworldInterop.inworld_TelemetryConfigBuilder_SetExporterType(m_DLLPtr, exporterType);
            InworldInterop.inworld_TelemetryConfigBuilder_SetLogLevel(m_DLLPtr, logLevel);
            InworldInterop.inworld_TelemetryConfigBuilder_SetSamplingRate(m_DLLPtr, samplingRate);
            InworldInterop.inworld_TelemetryConfigBuilder_SetSinkAbslLogs(m_DLLPtr, sinkAbslLogs);
            return new InworldTelemetryConfig(InworldInterop.inworld_TelemetryConfigBuilder_Build(m_DLLPtr));
        }
    }
}