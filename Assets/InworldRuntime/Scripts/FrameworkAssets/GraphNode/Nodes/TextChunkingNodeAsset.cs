/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Node;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Specialized node asset for text chunking operations within graph workflows in the Inworld framework.
    /// Extends the base node functionality to provide text segmentation and division capabilities.
    /// Input: InworldText or InworldDataStream&lt;string&gt;
    /// Output: InworldDataStream&lt;string&gt;
    /// This asset can be created through Unity's Create menu and used to break down large text into manageable chunks.
    /// Used for implementing text segmentation, content division, and text stream processing in AI workflows.
    /// </summary>
    // Input: InworldText or InworldDataStream<string>
    // Output: InworldDataStream<string>
    [CreateAssetMenu(fileName = "Node_TextChunking", menuName = "Inworld/Create Node/Text Operation/Text Chunking", order = -2498)]
    public class TextChunkingNodeAsset : InworldNodeAsset
    {
        public override string NodeTypeName => "TextChunkingNode";
        /// <summary>
        /// Creates the runtime representation of this text chunking node within the specified graph.
        /// Initializes the text chunking processing capabilities and creates the runtime node instance.
        /// Sets up the logic for breaking down text input into smaller, manageable chunks.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this text chunking node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            Runtime = new TextChunkingNode(NodeName, new NodeExecutionConfig());
            return Runtime?.IsValid ?? false;
        }
    }
}