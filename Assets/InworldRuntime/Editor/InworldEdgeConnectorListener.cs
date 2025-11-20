#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using EdgeView = UnityEditor.Experimental.GraphView.Edge;


namespace Inworld.Framework.Editor
{
    /// <summary>
    /// Listens to edge drag events. When an edge is dropped outside a port,
    /// opens the node search window and connects the newly created node back to the dragged port.
    /// </summary>
    public class InworldEdgeConnectorListener : IEdgeConnectorListener
    {
        readonly InworldGraphView m_GraphView;
        readonly InworldNodeSearchWindow m_SearchWindow;

        public InworldEdgeConnectorListener(InworldGraphView graphView, InworldNodeSearchWindow searchWindow)
        {
            m_GraphView = graphView;
            m_SearchWindow = searchWindow;
        }

        public void OnDropOutsidePort(EdgeView edge, Vector2 position)
        {
            Port startPort = edge.output != null ? edge.output : edge.input;
            if (startPort == null)
                return;
            m_SearchWindow.SetPendingEdgeContext(startPort, position);
            SearchWindow.Open(new SearchWindowContext(position), m_SearchWindow);
        }

        public void OnDrop(GraphView graphView, EdgeView edge)
        {
            
        }
    }
}
#endif