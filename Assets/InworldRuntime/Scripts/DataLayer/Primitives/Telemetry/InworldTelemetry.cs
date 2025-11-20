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
    /// Represents serializable telemetry configuration data for the Inworld framework.
    /// Contains all necessary parameters for configuring telemetry collection and export.
    /// Used as a data transfer object for telemetry settings that can be serialized and stored.
    /// </summary>
    [Serializable]
    public class InworldTelemetry
    {
        /// <summary>
        /// The name of the service for telemetry identification.
        /// </summary>
        public string serviceName;
        
        /// <summary>
        /// The version of the service for telemetry tracking.
        /// </summary>
        public string serviceVersion;
        
        /// <summary>
        /// The endpoint URL for telemetry data export.
        /// </summary>
        public string endPoint;
        
        /// <summary>
        /// The type of telemetry exporter to use.
        /// </summary>
        public int exporterType;
        
        /// <summary>
        /// The minimum log level for message filtering.
        /// </summary>
        public int logLevel;
        
        /// <summary>
        /// The sampling rate for distributed tracing (0.0 to 1.0).
        /// </summary>
        public double samplingRate;
        
        /// <summary>
        /// Whether to sink Abseil library logs into the telemetry system.
        /// </summary>
        public bool sinkAbslLogs;
        
        /// <summary>
        /// The API key for authenticating with telemetry services.
        /// </summary>
        public string telemetryKey;
        
        /// <summary>
        /// Gets a value indicating whether this telemetry configuration is valid.
        /// Checks that all required fields are properly configured for telemetry operations.
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(telemetryKey) && !string.IsNullOrEmpty(endPoint);
    }
}