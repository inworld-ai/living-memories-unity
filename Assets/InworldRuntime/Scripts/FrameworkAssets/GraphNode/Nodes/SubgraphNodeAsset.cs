using System.Collections.Generic;
using Inworld.Framework.Graph;
using UnityEngine;

namespace Inworld.Framework
{
    // Yan: Subgraph is a node rather than a graph.
    //      It can contain the details of the nodes/edges/conditions that are defined in the graph.
    [CreateAssetMenu(fileName = "Node_Subgraph", menuName = "Inworld/Create Graph/SubGraph", order = -800)]
    public class SubgraphNodeAsset : InworldNodeAsset
    {
        [SerializeField] InworldGraphAsset m_SubGraphAsset;
        
        public override bool CreateRuntime(InworldGraphAsset graphAsset)
        {
            if (m_SubGraphAsset?.CreateRuntime() ?? false)
                m_SubGraphAsset.CompileRuntime();
            return base.CreateRuntime(graphAsset);
        }
    }
}