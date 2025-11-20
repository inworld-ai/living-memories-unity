/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Framework
{
    /// <summary>
    /// A generic circular buffer data structure used to store microphone audio data efficiently.
    /// This buffer automatically overwrites old data when the capacity is reached, making it ideal for continuous audio streaming.
    /// </summary>
    /// <typeparam name="T">The type of data to store in the buffer.</typeparam>
    public class CircularBuffer<T>
    {
        /// <summary>
        /// The position of the last written data in the buffer.
        /// </summary>
        public int lastPos = 0;
        
        /// <summary>
        /// The current write position in the buffer.
        /// </summary>
        public int currPos = 0;

        readonly List<T> m_Buffer;
        readonly int m_Size;
        
        /// <summary>
        /// Initializes a new instance of the CircularBuffer class with the specified capacity.
        /// </summary>
        /// <param name="size">The maximum number of elements the buffer can hold.</param>
        public CircularBuffer(int size)
        {
            m_Buffer = new List<T>();
            for (int i = 0; i < size; i++)
                m_Buffer.Add(default);
            m_Size = size;
        }

        /// <summary>
        /// Clears all data in the buffer by setting all elements to their default values.
        /// Resets the buffer to its initial empty state.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < m_Size; i++)
            {
                m_Buffer[i] = default;
            }
        }
        
        /// <summary>
        /// Adds a list of objects to the buffer in a circular fashion.
        /// When the buffer capacity is exceeded, older data is automatically overwritten.
        /// </summary>
        /// <param name="objs">The list of objects to add to the buffer.</param>
        public void Enqueue(List<T> objs)
        {
            lastPos = currPos;
            int nIndex = lastPos;
            for (int i = 0; i < objs.Count; i++)
            {
                nIndex = (lastPos + i) % m_Size;
                m_Buffer[nIndex] = objs[i];
            }
            currPos = (nIndex + 1) % m_Size; 
        }
        
        /// <summary>
        /// Converts the entire buffer to a standard List.
        /// Returns all elements currently stored in the buffer, including default values.
        /// </summary>
        /// <returns>A List containing all buffer elements.</returns>
        public List<T> ToList() => m_Buffer;

        /// <summary>
        /// Retrieves a range of data from the buffer between the specified indices.
        /// Handles circular buffer wraparound automatically.
        /// </summary>
        /// <param name="start">The starting index (inclusive).</param>
        /// <param name="end">The ending index (exclusive).</param>
        /// <returns>A List containing the elements in the specified range, or an empty list if indices are invalid.</returns>
        public List<T> GetRange(int start, int end)
        {
            List<T> objs = new List<T>();
            if (start < 0 || start >= m_Size || end < 0 || end >= m_Size)
                return objs;
            if (end < start)
            {
                objs.AddRange(m_Buffer.GetRange(start, m_Buffer.Count - start));
                objs.AddRange(m_Buffer.GetRange(0, end));
            }
            else if (end > start)
            {
                objs.AddRange(m_Buffer.GetRange(start, currPos - start));
            }
            return objs;
        }
        
        /// <summary>
        /// Dequeues all data that was added since the last dequeue operation.
        /// Returns data between the last position and current position, handling circular wraparound.
        /// </summary>
        /// <returns>A List containing the newly added elements since the last dequeue.</returns>
        public List<T> Dequeue()
        {
            List<T> objs = new List<T>();
            if (currPos < lastPos)
            {
                objs.AddRange(m_Buffer.GetRange(lastPos, m_Buffer.Count - lastPos));
                objs.AddRange(m_Buffer.GetRange(0, currPos));
            }
            else if (currPos > lastPos)
            {
                objs.AddRange(m_Buffer.GetRange(lastPos, currPos - lastPos));
            }
            return objs;
        }
        
        /// <summary>
        /// Prints all buffer contents to the Unity console for debugging purposes.
        /// Each element is logged on a separate line using its ToString() method.
        /// </summary>
        public void Print()
        {
            foreach (T data in m_Buffer)
            {
                Debug.Log(data.ToString());
            }
        }
    }
}