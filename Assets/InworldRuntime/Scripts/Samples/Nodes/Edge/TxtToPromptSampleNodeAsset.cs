/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Graph;
using Inworld.Framework.LLM;
using UnityEngine;


namespace Inworld.Framework.Samples.Node
{
    /// <summary>
    /// Sample custom node asset that converts text inputs into LLM chat requests.
    /// Processes multiple text inputs by concatenating them and creating a user message for LLM interaction.
    /// Demonstrates how to build custom edge nodes that transform data between different formats.
    /// Serves as an example for creating text-to-prompt conversion functionality in graph workflows.
    /// </summary>
    public class TxtToPromptSampleNodeAsset : CustomNodeAsset
    {
        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            string strInput = "";
            if (inputs != null)
            {
                for (int i = 0; i < inputs.Size; i++)
                {
                    InworldText text = new InworldText(inputs[i]);
                    if (text.IsValid)
                    {
                        strInput += text.Text;
                    }
                }
            }
            InworldVector<InworldMessage> messages = new InworldVector<InworldMessage>();
            InworldMessage message = new InworldMessage();
            message.Role = Role.User;
            message.Content = strInput;
            messages.Add(message);
            return new LLMChatRequest(messages);
        }
    }
}