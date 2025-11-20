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
    /// Represents operation status information within the Inworld framework.
    /// Provides status codes and error details for tracking the success or failure
    /// of operations, API calls, and system processes.
    /// </summary>
    public class InworldStatus : InworldFrameworkDllClass
    {
        /// <summary>
        /// Initializes a new instance of the InworldStatus class with default (OK) status.
        /// Creates a new native status object representing a successful operation.
        /// </summary>
        public InworldStatus()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Status_new(), InworldInterop.inworld_Status_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the InworldStatus class with the specified status code and reason.
        /// Creates a status object representing a specific operation result with explanatory text.
        /// </summary>
        /// <param name="statusCode">The status code indicating the operation result.</param>
        /// <param name="reason">A descriptive message explaining the status or error condition.</param>
        public InworldStatus(StatusCode statusCode, string reason)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Status_createStatus(Convert.ToInt32(statusCode), reason), InworldInterop.inworld_Status_delete);
        }

        /// <summary>
        /// Initializes a new instance of the InworldStatus class from an existing native pointer.
        /// Used for wrapping existing native status objects created by the C++ library.
        /// </summary>
        /// <param name="dllPtr">Native pointer to an existing status instance.</param>
        public InworldStatus(IntPtr dllPtr)
        {
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_Status_delete);
        }

        /// <summary>
        /// Returns a string representation of the status information.
        /// Provides a human-readable description of the status including code and reason.
        /// </summary>
        /// <returns>A formatted string describing the status, or the base string representation if invalid.</returns>
        public override string ToString()
        {
            return m_DLLPtr == IntPtr.Zero ? base.ToString() : InworldInterop.inworld_Status_ToString(m_DLLPtr);
        }

        /// <summary>
        /// Gets a value indicating whether the status represents a successful operation.
        /// Returns true if the operation completed successfully without errors.
        /// </summary>
        /// <value>True if the status is OK (successful); otherwise, false.</value>
        public bool IsOK => m_DLLPtr != IntPtr.Zero && InworldInterop.inworld_Status_ok(m_DLLPtr);
    }
}