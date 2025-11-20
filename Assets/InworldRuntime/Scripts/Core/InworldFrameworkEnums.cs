/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

namespace Inworld.Framework
{
    /// <summary>
    /// Defines categories of potentially unsafe or sensitive content topics that can be detected and filtered.
    /// Used by safety systems to identify and control access to different types of sensitive content.
    /// </summary>
    public enum UnsafeTopic
    {
        /// <summary>
        /// Politics and political discussions.
        /// </summary>
        Politics,
        
        /// <summary>
        /// Content related to substance use and drugs.
        /// </summary>
        SubstanceUse,
        
        /// <summary>
        /// Religious content and discussions.
        /// </summary>
        Religion,
        
        /// <summary>
        /// Content related to self-harm, suicide, or self-injury.
        /// </summary>
        UnconditionalSelfHarm,
        
        /// <summary>
        /// Inappropriate content involving minors.
        /// </summary>
        UnconditionalSexualMinors,
        
        /// <summary>
        /// Content promoting hate groups or extremist organizations.
        /// </summary>
        UnconditionalHateGroup,
        
        /// <summary>
        /// Adult sexual content.
        /// </summary>
        AdultSexual,
        
        /// <summary>
        /// Violent content, descriptions of harm, or threatening language.
        /// </summary>
        Violence,
        
        /// <summary>
        /// Content related to alcohol consumption.
        /// </summary>
        Alcohol,
        
        /// <summary>
        /// Profane language, swear words, and obscenities.
        /// </summary>
        Profanity,
    }
    
    /// <summary>
    /// Specifies the type of AI model deployment to use for processing.
    /// Determines whether to use cloud-based services or local computation resources.
    /// </summary>
    public enum ModelType
    {
        /// <summary>
        /// Use remote cloud-based AI services for processing.
        /// </summary>
        Remote,
        
        /// <summary>
        /// Use local GPU acceleration for AI model processing.
        /// </summary>
        LocalGPU,
        
        /// <summary>
        /// Use local CPU-only processing for AI models.
        /// </summary>
        LocalCPU
    }
    
    /// <summary>
    /// Specifies the type of computing device to use for local AI model processing.
    /// Affects performance and compatibility based on available hardware.
    /// </summary>
    public enum DeviceType 
    {
        /// <summary>
        /// Use CPU for processing (compatible with all systems but slower).
        /// </summary>
        CPU,
        
        /// <summary>
        /// Use NVIDIA CUDA-compatible GPU for processing (requires NVIDIA GPU).
        /// </summary>
        CUDA,
        
        /// <summary>
        /// Use Apple Metal for GPU acceleration (macOS/iOS only).
        /// </summary>
        Metal
    }
    
    /// <summary>
    /// Defines the role of a participant in a conversation or interaction.
    /// Used to identify who is speaking or providing input in multi-party conversations.
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Human user providing input or asking questions.
        /// </summary>
        User,
        
        /// <summary>
        /// AI assistant providing responses and assistance.
        /// </summary>
        Assistant,
        
        /// <summary>
        /// Automated tool or function providing specific capabilities.
        /// </summary>
        Tool,
        
        /// <summary>
        /// System-level messages for configuration and control.
        /// </summary>
        System,
        
        /// <summary>
        /// Developer or administrator with special privileges.
        /// </summary>
        Developer
    }

    public enum ResponseFormat 
    {
        /** Text response format. */
        Text = 1,
        /** JSON response format.*/
        Json = 2,
        /** JSON schema response format.*/
        JsonSchema = 3
    };
    
    /// <summary>
    /// Defines standard status codes for operations and API responses.
    /// Based on gRPC status codes, used to indicate the result of operations within the Inworld framework.
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        Ok = 0,
        
        /// <summary>
        /// The operation was cancelled, typically by the caller.
        /// </summary>
        Cancelled = 1,
        
        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        Unknown = 2,
        
        /// <summary>
        /// The client specified an invalid argument.
        /// </summary>
        InvalidArgument = 3,
        
        /// <summary>
        /// The deadline expired before the operation could complete.
        /// </summary>
        DeadlineExceeded = 4,
        
        /// <summary>
        /// The requested entity was not found.
        /// </summary>
        NotFound = 5,
        
        /// <summary>
        /// The entity already exists and cannot be created.
        /// </summary>
        AlreadyExists = 6,
        
        /// <summary>
        /// The caller does not have permission to execute the specified operation.
        /// </summary>
        PermissionDenied = 7,
        
        /// <summary>
        /// Some resource has been exhausted (e.g., quota, disk space).
        /// </summary>
        ResourceExhausted = 8,
        
        /// <summary>
        /// The operation was rejected because the system is not in a state required for execution.
        /// </summary>
        FailedPrecondition = 9,
        
        /// <summary>
        /// The operation was aborted, typically due to a concurrency issue.
        /// </summary>
        Aborted = 10,
        
        /// <summary>
        /// The operation was attempted past the valid range.
        /// </summary>
        OutOfRange = 11,
        
        /// <summary>
        /// The operation is not implemented or supported.
        /// </summary>
        Unimplemented = 12,
        
        /// <summary>
        /// An internal error occurred.
        /// </summary>
        Internal = 13,
        
        /// <summary>
        /// The service is currently unavailable.
        /// </summary>
        Unavailable = 14,
        
        /// <summary>
        /// Unrecoverable data loss or corruption occurred.
        /// </summary>
        DataLoss = 15,
        
        /// <summary>
        /// The request does not have valid authentication credentials.
        /// </summary>
        Unauthenticated = 16,
        
        /// <summary>
        /// Other unspecified error conditions.
        /// </summary>
        Others = 20
    }

    public enum GraphEvent
    {
        None,
        Initializing,
        RegisterConfig,
        RegisterEdge,
        RegisterCustomNode,
        Initialized,
    }
}
