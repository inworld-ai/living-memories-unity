/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Framework.Event;
using Inworld.Framework.Goal;
using Inworld.Framework.Knowledge;
using Inworld.Framework.LLM;
using Inworld.Framework.Memory;
using Inworld.Framework.TTS;

namespace Inworld.Framework
{
    public interface IBaseDataVisitor
    {
        void Visit(InworldBaseData data);
        void Visit(InworldText text); //
        void Visit(InworldAudio audio); //
        void Visit(InworldError error);  //
        void Visit(MatchedKeywords keyword); //
        void Visit(ClassificationResult result); //
        void Visit(InworldJson json); //
        void Visit(InworldDataStream<string> stringStream); //
        void Visit(InworldDataStream<TTSOutput> ttsOutputStream); //
        void Visit(InworldDataStream<InworldContent> contentStream); //
        void Visit(MemoryState data); //
        void Visit(InworldCustomDataWrapper customDataWrapper); //
        void Visit(EventHistory eventHistory);  //
        void Visit(KnowledgeRecords knowledgeRecords); //
        void Visit(GoalAdvancement goal); //
        void Visit(LLMChatRequest llmChatRequest); //
        void Visit(LLMChatResponse llmChatResponse); //
        void Visit(LLMCompletionResponse llmCompletionResponse); //
        void Visit(SafetyResult safetyResult); //
        void Visit(MatchedIntents intents); //
    }


    public class InworldBaseData : InworldFrameworkDllClass
    {
        public InworldBaseData()
        {
        }

        public InworldBaseData(IntPtr dllPtr) =>
            m_DLLPtr = MemoryManager.Register(dllPtr, InworldInterop.inworld_BaseData_delete);

        public override string ToString()
        {
            return m_DLLPtr == IntPtr.Zero
                ? base.ToString()
                : InworldInterop.inworld_BaseData_ToString(m_DLLPtr);
        }

        public override bool IsValid => InworldInterop.inworld_BaseData_is_valid(m_DLLPtr);

        public virtual void Accept(IBaseDataVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}