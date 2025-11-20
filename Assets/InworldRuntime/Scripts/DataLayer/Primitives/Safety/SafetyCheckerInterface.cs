/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Safety
{
    /// <summary>
    /// Interface for safety checking functionality in the Inworld framework.
    /// This class provides methods to check if text content is safe according to configured safety policies.
    /// </summary>
    public class SafetyCheckerInterface : InworldInterface
    {
        /// <summary>
        /// Initializes a new instance of the SafetyCheckerInterface class from an existing native pointer.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing safety checker interface instance.</param>
        public SafetyCheckerInterface(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_SafetyCheckerInterface_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this safety checker interface is valid and ready for use.
        /// </summary>
        /// <value>True if the interface is valid; otherwise, false.</value>
        public override bool IsValid => InworldInterop.inworld_SafetyCheckerInterface_is_valid(m_DLLPtr);

        /// <summary>
        /// Checks if the provided sentence is safe according to default safety policies.
        /// </summary>
        /// <param name="sentence">The text sentence to check for safety violations.</param>
        /// <returns>True if the sentence is considered safe; otherwise, false.</returns>
        public bool IsSafe(string sentence)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_SafetyCheckerInterface_CheckSafety_rcstd_string(m_DLLPtr, sentence),
                InworldInterop.inworld_StatusOr_CheckSafetyResponse_status,
                InworldInterop.inworld_StatusOr_CheckSafetyResponse_ok,
                InworldInterop.inworld_StatusOr_CheckSafetyResponse_value,
                InworldInterop.inworld_StatusOr_CheckSafetyResponse_delete
            );
            if (result == IntPtr.Zero)
                return false;
            CheckSafetyResponse response = new CheckSafetyResponse(result);
            return response.IsValid && response.IsSafe;
        }
        
        /// <summary>
        /// Checks if the provided sentence is safe according to the specified safety configuration.
        /// </summary>
        /// <param name="sentence">The text sentence to check for safety violations.</param>
        /// <param name="config">The safety configuration to use for the safety check.</param>
        /// <returns>True if the sentence is considered safe according to the config; otherwise, false.</returns>
        public bool IsSafe(string sentence, SafetyConfig config)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                InworldInterop.inworld_SafetyCheckerInterface_CheckSafety_rcstd_string_rcinworld_SafetyConfig(m_DLLPtr, sentence, config.ToDLL),
                InworldInterop.inworld_StatusOr_CheckSafetyResponse_status,
                InworldInterop.inworld_StatusOr_CheckSafetyResponse_ok,
                InworldInterop.inworld_StatusOr_CheckSafetyResponse_value,
                InworldInterop.inworld_StatusOr_CheckSafetyResponse_delete
            );
            if (result == IntPtr.Zero)
                return false;
            CheckSafetyResponse response = new CheckSafetyResponse(result);
            return response.IsValid && response.IsSafe;
        }
    }
}