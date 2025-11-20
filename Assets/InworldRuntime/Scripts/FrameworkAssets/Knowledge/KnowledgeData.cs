/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inworld.Framework.Knowledge
{
    /// <summary>
    /// Represents a knowledge entry with title and content within the Inworld framework.
    /// Contains structured information that can be referenced and compiled for AI knowledge systems.
    /// Used for defining factual information, context, and reference material for AI characters.
    /// </summary>
    [Serializable]
    public class Knowledges
    {
        /// <summary>
        /// The title or identifier for this knowledge entry.
        /// Used to categorize and reference the specific knowledge item.
        /// </summary>
        public string title;
        
        /// <summary>
        /// The content of this knowledge entry as a list of strings.
        /// Contains the actual information, facts, or context data for this knowledge item.
        /// </summary>
        public List<string> content = new List<string>();
        
        /// <summary>
        /// Compiles this knowledge entry into a formatted list using the Inworld knowledge system.
        /// Processes the title and content through the framework's knowledge compilation pipeline.
        /// </summary>
        /// <returns>A compiled list of knowledge strings, or null if the knowledge system is unavailable.</returns>
        public List<string> CompileKnowledge()
        {
            return !InworldController.Knowledge ? null : InworldController.Knowledge.CompileKnowledges(title, content);
        }
    }
    
    /// <summary>
    /// ScriptableObject that manages a collection of knowledge entries for AI character systems.
    /// This asset can be created through Unity's Create menu and used to configure character knowledge.
    /// Used for defining, managing, and compiling knowledge bases for AI characters and conversation systems.
    /// </summary>
    [CreateAssetMenu(fileName = "KnowledgeData", menuName = "Inworld/Create Data/Knowledges", order = -2499)]
    public class KnowledgeData : ScriptableObject
    {
        /// <summary>
        /// The collection of knowledge entries managed by this asset.
        /// Contains all knowledge items with their titles and content.
        /// </summary>
        public List<Knowledges> knowledges = new List<Knowledges>();

        /// <summary>
        /// Gets a list of all knowledge identifiers (titles) in this knowledge data.
        /// Used for referencing and managing knowledge entries by their unique identifiers.
        /// </summary>
        public List<string> IDs => knowledges.Select(x => x.title).ToList();
        
        /// <summary>
        /// Compiles all knowledge entries in this collection into a unified knowledge base.
        /// Processes all knowledge items through the framework's compilation system.
        /// </summary>
        /// <returns>A compiled list of all knowledge strings, or null if the knowledge system is unavailable.</returns>
        public List<string> CompileKnowledges()
        {
            if (!InworldController.Knowledge)
                return null;
            List<string> output = new List<string>();
            foreach (Knowledges knowledge in knowledges)
            {
                output.AddRange(knowledge.CompileKnowledge());
            }
            return output;
        }

        /// <summary>
        /// Adds a new knowledge entry to the collection and recompiles the knowledge base.
        /// Includes the new knowledge in the managed collection and triggers compilation.
        /// </summary>
        /// <param name="newKnowledge">The new knowledge entry to add to the collection.</param>
        /// <returns>A compiled list of all knowledge strings after adding the new entry.</returns>
        public List<string> AddKnowledge(Knowledges newKnowledge)
        {
            knowledges.Add(newKnowledge);
            return CompileKnowledges();
        }
        
        /// <summary>
        /// Adds or updates knowledge content for a specific knowledge identifier.
        /// Creates a new knowledge entry if the ID doesn't exist, or adds content to an existing entry.
        /// </summary>
        /// <param name="knowledgeID">The identifier for the knowledge entry. Will be prefixed with "knowledge/" if not already.</param>
        /// <param name="content">The content string to add to the knowledge entry.</param>
        /// <returns>A compiled list of all knowledge strings after adding the content.</returns>
        public List<string> AddKnowledge(string knowledgeID, string content)
        {
            if (string.IsNullOrEmpty(knowledgeID))
                knowledgeID = "knowledge/new";
            else if (!knowledgeID.StartsWith("knowledge/"))
                knowledgeID = $"knowledge/{knowledgeID}";
            Knowledges knowledge = knowledges.FirstOrDefault(k => k.title == knowledgeID);
            if (knowledge == null)
            {
                Knowledges newKnowledge = new Knowledges
                {
                    title = knowledgeID,
                    content = new List<string>{content}
                };
                knowledges.Add(newKnowledge);
            }
            else
                knowledge.content.Add(content);
            return CompileKnowledges();
        }
        
        /// <summary>
        /// Adds or updates knowledge content for a specific knowledge identifier with multiple content items.
        /// Creates a new knowledge entry if the ID doesn't exist, or adds content to an existing entry.
        /// </summary>
        /// <param name="knowledgeID">The identifier for the knowledge entry. Will be prefixed with "knowledge/" if not already.</param>
        /// <param name="content">The list of content strings to add to the knowledge entry.</param>
        /// <returns>A compiled list of all knowledge strings after adding the content.</returns>
        public List<string> AddKnowledge(string knowledgeID, List<string> content)
        {
            if (string.IsNullOrEmpty(knowledgeID))
                knowledgeID = "knowledge/new";
            else if (!knowledgeID.StartsWith("knowledge/"))
                knowledgeID = $"knowledge/{knowledgeID}";
            Knowledges knowledge = knowledges.FirstOrDefault(k => k.title == knowledgeID);
            if (knowledge == null)
            {
                Knowledges newKnowledge = new Knowledges
                {
                    title = knowledgeID,
                    content = new List<string>(content)
                };
                knowledges.Add(newKnowledge);
            }
            else
                knowledge.content.AddRange(content);
            return CompileKnowledges();
        }
        
        /// <summary>
        /// Removes a knowledge entry from the collection by its title identifier.
        /// Also removes the knowledge from the framework's knowledge system if available.
        /// </summary>
        /// <param name="title">The title identifier of the knowledge entry to remove.</param>
        public void RemoveKnowledge(string title)
        {
            knowledges.RemoveAll(k => k.title == title);
            if (!InworldController.Knowledge)
                return;
            InworldController.Knowledge?.RemoveKnowledge(title);
        }

        /// <summary>
        /// Retrieves the compiled knowledge data for all knowledge entries in this collection.
        /// Fetches the processed knowledge information from the framework's knowledge system.
        /// </summary>
        /// <returns>A list of compiled knowledge strings for all entries, or null if the knowledge system is unavailable.</returns>
        public List<string> GetKnowledges()
        {
            return !InworldController.Knowledge ? null : InworldController.Knowledge.GetKnowledges(knowledges.Select(k => k.title).ToList());
        }
    }
}