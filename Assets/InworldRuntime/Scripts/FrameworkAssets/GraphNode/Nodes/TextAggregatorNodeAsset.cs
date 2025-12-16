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
    // Input: InworldDataStream<string>
    // Output: InworldText
    
    /// <summary>
    /// Specialized node asset for text aggregation operations within graph workflows in the Inworld framework.
    /// Extends the base node functionality to provide text collection and consolidation capabilities.
    /// Input: InworldDataStream&lt;string&gt;
    /// Output: InworldText
    /// This asset can be created through Unity's Create menu and used to combine multiple text streams into single text objects.
    /// Used for implementing text consolidation, stream-to-text conversion, and text data aggregation in AI workflows.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_TextAggregator", menuName = "Inworld/Create Node/Text Operation/Text Aggregator", order = -2499)]
    public class TextAggregatorNodeAsset : InworldNodeAsset
    {
        /// <summary>
        /// Creates the runtime representation of this text aggregator node within the specified graph.
        /// Initializes the text aggregation processing capabilities and creates the runtime node instance.
        /// Sets up the logic for combining multiple text streams into consolidated text output.
        /// </summary>
        /// <param name="graphAsset">The graph asset that will contain this text aggregator node.</param>
        /// <returns>True if runtime creation succeeded; otherwise, false.</returns>
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            m_Graph = graphAsset;
            Runtime = new TextAggregatorNode(NodeName, new NodeExecutionConfig());
            return Runtime?.IsValid ?? false;
        }
    }
}