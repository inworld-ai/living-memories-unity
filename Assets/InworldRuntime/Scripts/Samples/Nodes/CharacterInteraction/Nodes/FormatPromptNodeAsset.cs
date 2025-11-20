/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using UnityEngine;
using Inworld.Framework.Graph;
using Inworld.Framework.LLM;

using Newtonsoft.Json;

namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Custom node asset for formatting conversation prompts using Jinja templating.
    /// Processes character conversation data through template rendering to create structured LLM chat requests.
    /// Converts conversation context and character data into properly formatted prompts for AI processing.
    /// This asset can be created through Unity's Create menu for character conversation graphs.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_FormatPrompt", menuName = "Inworld/Create Node/Character Conversation/Format Prompt", order = -2899)]
    public class FormatPromptNodeAsset : CustomNodeAsset
    {
        [SerializeField] bool m_IsDebug = false;
        [Tooltip("If checked, it'll use the prompt below or graph asset's default prompt. Otherwise it will use the incoming InworldText")]
        [SerializeField] bool m_UseDefaultPrompt;
        [Tooltip("If Prompt is defined, the input data from last node will be discarded unless it's InworldJson")]
        [TextArea(5, 20)] [SerializeField] string m_Prompt;
        public override string NodeTypeName => "FormatPromptNode";
        
        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            if (!m_UseDefaultPrompt)
            {
                if (inputs.Size == 0)
                {
                    return new InworldError("No input data", StatusCode.DataLoss);
                }
                InworldBaseData inputData = inputs[0];
                string outResult = TryProcessText(inputData);
                if (string.IsNullOrEmpty(outResult))
                    outResult = TryProcessLLMResponse(inputData);
                if (!string.IsNullOrEmpty(outResult)) 
                    return GenerateLLMRequest(outResult);
                LLMChatRequest llmRequest = new LLMChatRequest(inputData);
                if (llmRequest.IsValid)
                    return llmRequest;
                return new InworldError("Data type mismatch", StatusCode.DataLoss);
            }

            if (!string.IsNullOrEmpty(m_Prompt))
            {
                if (inputs.Size == 0) 
                    return new InworldError("Graph Mismatch", StatusCode.InvalidArgument);
                InworldBaseData inputData = inputs[0];
                InworldJson jsonData = new InworldJson(inputData);
                if (!jsonData.IsValid) 
                    return new InworldError("Graph Mismatch", StatusCode.InvalidArgument);
                string json = jsonData.ToString();
                string jinjaPrompt = InworldFrameworkUtil.RenderJinja(m_Prompt, json);
                return GenerateLLMRequest(jinjaPrompt);
            }

            if (m_Graph is CharacterInteractionGraphAsset charGraph)
            {
                return GenerateLLMRequestByDefaultPrompt(charGraph);
            }

            return new InworldError("Graph Mismatch", StatusCode.InvalidArgument);
        }

        InworldBaseData GenerateLLMRequest(string outResult)
        {
            InworldVector<InworldMessage> messages = new InworldVector<InworldMessage>();
            InworldMessage message = new InworldMessage();
            message.Role = Role.User;
            message.Content = outResult;
            messages.Add(message);
            if (m_IsDebug || InworldFrameworkUtil.IsDebugMode)
                Debug.Log($"{message.Content}");
            return new LLMChatRequest(messages);
        }

        string TryProcessLLMResponse(InworldBaseData inputData)
        {
            LLMChatResponse llmResponse = new LLMChatResponse(inputData);
            return llmResponse.IsValid ? llmResponse.Content.Content : "";
        }

        string TryProcessText(InworldBaseData inputData)
        {
            InworldText txtResult = new InworldText(inputData);
            return txtResult.IsValid ? txtResult.Text : "";
        }

        InworldBaseData GenerateLLMRequestByDefaultPrompt(CharacterInteractionGraphAsset charGraph)
        {
            string json = JsonConvert.SerializeObject(charGraph.prompt.conversationData);
            string data = InworldFrameworkUtil.RenderJinja(charGraph.prompt.prompt, json);
            if (!string.IsNullOrEmpty(data))
                charGraph.prompt.jinjaPrompt = data;
            InworldVector<InworldMessage> messages = new InworldVector<InworldMessage>();
            InworldMessage message = new InworldMessage();
            message.Role = Role.User;
            message.Content = charGraph.prompt.jinjaPrompt;
            messages.Add(message);
            if (m_IsDebug || InworldFrameworkUtil.IsDebugMode)
                Debug.Log($"{message.Content}");
            return new LLMChatRequest(messages);
        }
    }
}