/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Node
{
    /// <summary>
    /// Represents a base node in a graph structure that can process data within the Inworld framework.
    /// Serves as the foundation for all node types in graph-based AI workflows.
    /// Used for implementing custom processing logic and data transformation operations.
    /// </summary>
    public class InworldNode : InworldFrameworkDllClass
    {
        // For child class to overwrite the MemoryRegister.
        /// <summary>
        /// Initializes a new instance of the InworldNode class with default settings.
        /// This constructor allows child classes to override memory registration behavior.
        /// </summary>
        public InworldNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the InworldNode class from a native pointer.
        /// </summary>
        /// <param name="dllPtr">The native pointer to the node object.</param>
        public InworldNode(IntPtr dllPtr) =>
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_Node_delete);
        
        /// <summary>
        /// Gets the unique identifier of this node within the graph structure.
        /// Used for referencing and connecting nodes in graph operations.
        /// </summary>
        public virtual string ID => InworldInterop.inworld_Node_id(m_DLLPtr);
        
        // Yan: Skip Config Request as it's currently returning SWIGTYPE_p_std__optionalT_std__reference_wrapperT_inworld__workflows__NodeConfig_const_t_t

        /// <summary>
        /// Gets a value indicating whether this node is in a valid state for execution.
        /// Overrides the base class implementation to provide node-specific validation.
        /// </summary>
        public override bool IsValid => InworldInterop.inworld_Node_is_valid(m_DLLPtr);

        /// <summary>
        /// Processes input data through this node and returns the result.
        /// This is the main execution method for node-based data processing.
        /// </summary>
        /// <param name="processContext"></param>
        /// <param name="parameter">A vector containing the input data to process.</param>
        /// <returns>The processed data result, or null if processing fails.</returns>
        public virtual InworldBaseData Process(ProcessContext processContext, InworldVector<InworldBaseData> parameter)
        {
            IntPtr result = InworldFrameworkUtil.Execute(
                    InworldInterop.inworld_Node_Process(m_DLLPtr, processContext.ToDLL, parameter.ToDLL), 
                    InworldInterop.inworld_StatusOr_BaseData_status,
                    InworldInterop.inworld_StatusOr_BaseData_ok,
                    InworldInterop.inworld_StatusOr_BaseData_value,
                    InworldInterop.inworld_StatusOr_BaseData_delete
                    );
            return result != IntPtr.Zero ? new InworldBaseData(result) : null;
        }
    }
}