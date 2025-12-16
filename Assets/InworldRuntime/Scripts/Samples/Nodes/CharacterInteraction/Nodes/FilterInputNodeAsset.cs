/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Framework.Graph;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    // Input: InworldText or InworldAudio
    // Output: InworldText or InworldAudio or InworldError
    
    // YAN: This is the first node of the Character interaction graph.
    //      Input: Audio or Text
    //      Output: If Audio, Send to STT. If Text, directly return as output.
    //      Next Node: Attach the message to the prompt.
    
    /// <summary>
    /// Custom node asset that serves as the entry point for character interaction graphs.
    /// Filters and validates input data, supporting both audio and text inputs.
    /// Routes audio data for speech-to-text processing and passes text data directly through.
    /// Acts as the initial processing stage in character conversation workflows.
    /// This asset can be created through Unity's Create menu for character conversation graphs.
    /// </summary>
    [CreateAssetMenu(fileName = "Node_FormatPrompt", menuName = "Inworld/Create Node/Character Conversation/Filter Input", order = -2899)]
    public class FilterInputNodeAsset : CustomNodeAsset
    {
        public override string NodeTypeName => "FilterInputNode";

        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            if (inputs.Size == 0)
            {
                return new InworldError("No input data", StatusCode.DataLoss);
            }
            InworldBaseData inputData = inputs[0]; // YAN: Let's only process the last single input.
            InworldText textResult = new InworldText(inputData);
            if (textResult.IsValid)
                return textResult;

            InworldAudio audioResult = new InworldAudio(inputData);
            if (audioResult.IsValid)
                return audioResult;
            
            return new InworldError($"Unsupported data type: {inputData.GetType()}", StatusCode.Unimplemented);
        }
    }
}