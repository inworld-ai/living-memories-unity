/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

namespace Inworld.Framework.Event
{
    /// <summary>
    /// Represents a chronological collection of events within the Inworld framework.
    /// Maintains a history of interactions, actions, and state changes for tracking, analysis, and replay purposes.
    /// Used for conversation history, session logging, and behavioral analysis.
    /// </summary>
    public class EventHistory : InworldBaseData
    {
        /// <summary>
        /// Initializes a new instance of the EventHistory class with the specified collection of events.
        /// Creates an event history containing the provided sequence of events in chronological order.
        /// </summary>
        /// <param name="events">A vector of InworldEvent objects representing the chronological event sequence.</param>
        public EventHistory(InworldVector<InworldEvent> events)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_EventHistory_new(events.ToDLL), InworldInterop.inworld_EventHistory_delete);
        }
        
        /// <summary>
        /// Initializes a new instance of the EventHistory class from an existing native pointer.
        /// Used for wrapping existing native event history objects created by the C++ library.
        /// </summary>
        /// <param name="rhs">Native pointer to an existing event history instance.</param>
        public EventHistory(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_EventHistory_delete);
        }

        /// <summary>
        /// Initializes a new instance of the EventHistory class by converting from base data.
        /// Extracts event history data from a generic InworldBaseData object.
        /// </summary>
        /// <param name="baseData">The base data object to convert to event history format.</param>
        public EventHistory(InworldBaseData baseData)
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_BaseDataAs_EventHistory(baseData.ToDLL), InworldInterop.inworld_BaseData_delete);
        }

        /// <summary>
        /// Gets a value indicating whether this event history contains valid data.
        /// </summary>
        /// <value>True if the event history is valid and contains meaningful event data; otherwise, false.</value>
        public override bool IsValid => InworldInterop.inworld_EventHistory_is_valid(m_DLLPtr);
        
        /// <summary>
        /// Returns a string representation of the event history.
        /// Provides a human-readable summary of the events contained in this history.
        /// </summary>
        /// <returns>A formatted string describing the event history, or empty string if invalid.</returns>
        public override string ToString() => InworldInterop.inworld_EventHistory_ToString(m_DLLPtr);
        
        /// <summary>
        /// Gets the collection of events contained in this history.
        /// Returns all events in chronological order, allowing for analysis and replay of interactions.
        /// </summary>
        /// <value>An InworldVector containing the chronological sequence of events.</value>
        public InworldVector<InworldEvent> Events => new InworldVector<InworldEvent>(InworldInterop.inworld_EventHistory_events(m_DLLPtr));

        /// <summary>
        /// Accepts a visitor for processing this event history data object.
        /// Part of the visitor pattern implementation for data processing and analysis.
        /// </summary>
        /// <param name="visitor">The visitor to accept for processing this event history.</param>
        public override void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}