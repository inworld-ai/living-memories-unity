using System;

namespace Inworld.Framework
{
    public class ChatMessage : InworldFrameworkDllClass
    {
        public ChatMessage()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_ChatMessage_new(),
                InworldInterop.inworld_ChatMessage_delete);
        }

        public ChatMessage(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_ChatMessage_delete);
        }

        public string Role
        {
            get => InworldInterop.inworld_ChatMessage_role_get(m_DLLPtr);
            set => InworldInterop.inworld_ChatMessage_role_set(m_DLLPtr, value);
        }

        public bool HasMultimodalContent => InworldInterop.inworld_ChatMessage_HasMultimodalContent(m_DLLPtr);
        
        public bool HasSingleContent => InworldInterop.inworld_ChatMessage_HasSingleContent(m_DLLPtr);

        public void InitMultimodalContent() => InworldInterop.inworld_ChatMessage_InitMultimodalContent(m_DLLPtr);

        public void SetTextContent(string textContent) =>
            InworldInterop.inworld_ChatMessage_SetTextContent(m_DLLPtr, textContent);
        
        public void SetTemplateContent(string templateContent) =>
            InworldInterop.inworld_ChatMessage_SetTemplateContent(m_DLLPtr, templateContent);
        
        public void AddTextToMultimodal(string textContent) =>
            InworldInterop.inworld_ChatMessage_AddTextToMultimodal(m_DLLPtr, textContent);
        
        public void AddTemplateToMultimodal(string templateContent) =>
            InworldInterop.inworld_ChatMessage_AddTemplateToMultimodal(m_DLLPtr, templateContent);
        
        public void AddImageToMultimodal(string imageUrl) =>
            InworldInterop.inworld_ChatMessage_AddImageToMultimodal_rcstd_string(m_DLLPtr, imageUrl);
        
        public void AddImageToMultimodal(string imageUrl, string detail) =>
            InworldInterop.inworld_ChatMessage_AddImageToMultimodal_rcstd_string_rcstd_string(m_DLLPtr, imageUrl, detail);
        
        public void AddImageTemplateToMultimodal(string imageTemplateUrl) =>
            InworldInterop.inworld_ChatMessage_AddImageTemplateToMultimodal_rcstd_string(m_DLLPtr, imageTemplateUrl);

        public void AddImageTemplateToMultimodal(string imageTemplateUrl, string detail) =>
            InworldInterop.inworld_ChatMessage_AddImageTemplateToMultimodal_rcstd_string_rcstd_string(m_DLLPtr, imageTemplateUrl, detail);
        
        public int MultimodalItemCount => InworldInterop.inworld_ChatMessage_GetMultimodalItemCount(m_DLLPtr);
    }
}