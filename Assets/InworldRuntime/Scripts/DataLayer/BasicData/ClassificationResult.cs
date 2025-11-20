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
    /// Represents the result of a classification operation within the Inworld framework.
    /// Contains classified categories or labels determined by AI analysis of input data.
    /// Used for content categorization, intent detection, and automated classification tasks.
    /// </summary>
    public class ClassificationResult : InworldBaseData
    {
        /// <summary>
        /// Initializes a new instance of the ClassificationResult class with the specified class names.
        /// Creates a classification result containing the provided categories or labels.
        /// </summary>
        /// <param name="classes">A vector of class name strings representing the classification categories.</param>
        public ClassificationResult(InworldVector<string> classes)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ClassificationResult_new(classes.ToDLL),
                InworldInterop.inworld_ClassificationResult_delete);
        }

        /// <summary>
        /// Initializes a new instance of the ClassificationResult class from an existing native pointer.
        /// Used for wrapping existing native classification result objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing classification result instance.</param>
        public ClassificationResult(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ClassificationResult_delete);
        }

        /// <summary>
        /// Initializes a new instance of the ClassificationResult class by converting from base data.
        /// Extracts classification result data from a generic InworldBaseData object.
        /// </summary>
        /// <param name="parent">The base data object to convert to classification result format.</param>
        public ClassificationResult(InworldBaseData parent)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_ClassificationResult(parent.ToDLL),
                InworldInterop.inworld_ClassificationResult_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this classification result contains valid data.
        /// </summary>
        /// <value>True if the classification result is valid and contains meaningful data; otherwise, false.</value>
        public override bool IsValid => InworldInterop.inworld_ClassificationResult_is_valid(m_DLLPtr);

        /// <summary>
        /// Returns a string representation of the classification result.
        /// Provides a human-readable summary of the classified categories.
        /// </summary>
        /// <returns>A formatted string describing the classification result, or empty string if invalid.</returns>
        public override string ToString() => InworldInterop.inworld_ClassificationResult_ToString(m_DLLPtr);

        /// <summary>
        /// Gets a value indicating whether this result contains any classification categories.
        /// Useful for checking if the classification operation produced any results.
        /// </summary>
        /// <value>True if the result contains one or more classification categories; otherwise, false.</value>
        public bool HasClasses => InworldInterop.inworld_ClassificationResult_HasClasses(m_DLLPtr);

        /// <summary>
        /// Gets the collection of classification category names.
        /// Returns all categories or labels identified during the classification process.
        /// </summary>
        /// <value>An InworldVector containing the classification category names as strings.</value>
        public InworldVector<string> Classes => new InworldVector<string>(InworldInterop.inworld_ClassificationResult_classes(m_DLLPtr));
        
        /// <summary>
        /// Accepts a visitor for processing this classification result data object.
        /// Part of the visitor pattern implementation for data processing and analysis.
        /// </summary>
        /// <param name="visitor">The visitor to accept for processing this classification result.</param>
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}