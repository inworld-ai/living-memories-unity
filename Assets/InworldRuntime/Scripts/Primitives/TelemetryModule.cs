/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Telemetry;
using UnityEngine;

namespace Inworld.Framework.Primitive
{
    /// <summary>
    /// Module for telemetry and logging functionality within the Inworld framework.
    /// Manages the collection and reporting of system metrics, performance data, and usage statistics.
    /// Provides centralized configuration and setup for telemetry services across the framework.
    /// Note: This module does not inherit from InworldFrameworkModule and has a simpler lifecycle.
    /// </summary>
    public class TelemetryModule : MonoBehaviour
    {
        [SerializeField] InworldTelemetry m_TelemetryConfig;
        InworldTelemetryBuilder m_Builder;
        InworldTelemetryConfig m_Config;

        /// <summary>
        /// Builds and initializes the telemetry system using the configured settings.
        /// Creates a telemetry builder, configures the telemetry system, and enables data collection.
        /// Falls back to framework default configuration if no valid configuration is provided.
        /// </summary>
        public void Build()
        {
            if (!m_TelemetryConfig.IsValid)
                m_TelemetryConfig = InworldFrameworkUtil.TelemetryConfig;
            if (!m_TelemetryConfig.IsValid)
                return;
            m_Builder?.Dispose();
            m_Builder = new InworldTelemetryBuilder();
            m_Config?.Dispose();
            m_Config = m_Builder.Build(m_TelemetryConfig);
            m_Config.Config();
            Debug.Log("Telemetry Setup Completed");
        }

        void OnDestroy()
        {
            m_Config?.Dispose();
            m_Builder?.Dispose();
        }
    }
}