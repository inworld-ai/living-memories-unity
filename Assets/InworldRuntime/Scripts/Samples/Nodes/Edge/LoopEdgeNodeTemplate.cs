using System.Collections.Generic;
using Inworld.Framework;
using Inworld.Framework.Graph;

using Inworld.Framework.Samples.Node;
using Inworld.Framework.Samples.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoopEdgeNodeTemplate : NodeTemplate
{
    [SerializeField] TextCombinerNodeAsset m_CombinerAsset;
    [SerializeField] LoopEdgeAsset m_LoopEdgeAsset;
    [SerializeField] TMP_InputField m_InputField;
    [SerializeField] RectTransform m_ContentAnchor;
    [SerializeField] ChatBubble m_BubbleLeft;
    [SerializeField] ChatBubble m_BubbleRight;
    
    protected readonly List<ChatBubble> m_Bubbles = new List<ChatBubble>();
    List<InworldUIElement> m_UIElements = new List<InworldUIElement>();
    InworldVector<InworldMessage> m_Messages = new InworldVector<InworldMessage>();

    int m_nCurrentQuery = 0;
    void Awake()
    {
        m_nCurrentQuery = 0;
        m_UIElements.AddRange(GetComponentsInChildren<InworldUIElement>(true));
    }
        
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) && !string.IsNullOrEmpty(m_InputField.text))
            SendText();
    }
    
    protected override void OnGraphCompiled(InworldGraphAsset obj)
    {
        foreach (InworldUIElement element in m_UIElements)
            element.Interactable = true;
    }
    
    protected override void OnGraphResult(InworldBaseData obj)
    {
        InworldText response = new InworldText(obj);
        if (response.IsValid)
        {
            string message = response.Text;
            InsertBubble(m_BubbleLeft, Role.User.ToString(), message);
        }
    }
    
    /// <summary>
    /// Forces a layout rebuild of the chat content area.
    /// Ensures proper display and positioning of chat bubbles after content changes.
    /// </summary>
    public void UpdateContent() => LayoutRebuilder.ForceRebuildLayoutImmediate(m_ContentAnchor);
    
    /// <summary>
    /// Sends the current text input through the node connection chain for processing.
    /// Creates a user message bubble and submits the text to the graph executor for node processing.
    /// Clears the input field after submission.
    /// </summary>
    public async void SendText()
    {
        m_nCurrentQuery++;
        m_LoopEdgeAsset.echoTimes = m_nCurrentQuery;
        m_CombinerAsset.currentText = "";
        InworldText messages = new InworldText(m_InputField.text);
        m_InputField.text = "";
        await m_InworldGraphExecutor.ExecuteGraphAsync("Loop", messages);
    }
    
    protected virtual void InsertBubble(ChatBubble bubble, string speaker, string content, int index = -1)
    {
        if (index == -1 || index >= m_Bubbles.Count)
        {
            ChatBubble outBubble = Instantiate(bubble, m_ContentAnchor);
            outBubble.SetBubble(speaker, InworldFrameworkUtil.PlayerIcon, content);
            m_Bubbles.Add(outBubble);
        }
        else
        {
            ChatBubble outBubble = m_Bubbles[index];
            outBubble.SetBubble(speaker, InworldFrameworkUtil.InworldIcon, content);
        }
        UpdateContent();
    }
    
    /// <summary>
    /// Clears all conversation history and removes all chat bubbles.
    /// Resets both the visual chat interface and the internal message history for a fresh start.
    /// </summary>
    public void ClearHistory()
    {
        m_nCurrentQuery = 0;
        while (m_Bubbles.Count > 0)
        {
            ChatBubble bubble = m_Bubbles[0];
            m_Bubbles.RemoveAt(0);
            DestroyImmediate(bubble.gameObject);
        }
        m_Messages.Clear();
    }
}
