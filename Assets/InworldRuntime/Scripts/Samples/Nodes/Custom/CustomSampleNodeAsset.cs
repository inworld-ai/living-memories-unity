/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

using Inworld.Framework.Graph;
using Inworld.Framework.Node;

namespace Inworld.Framework.Samples.Node
{
    // Input: InworldText or InworldAudio
    // Output: InworldText or InworldAudio or InworldError
    
    /// <summary>
    /// Sample custom node asset demonstrating basic data processing capabilities within the Inworld framework.
    /// Processes both text and audio inputs with simple transformations (text to uppercase, audio volume reduction).
    /// Shows how to create custom node logic with error handling and type-specific processing.
    /// Serves as a template for building custom data processing nodes in graph workflows.
    /// </summary>
    public class CustomSampleNodeAsset : CustomNodeAsset
    {
        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            if (inputs.Size == 0)
            {
                return new InworldError("No input data", StatusCode.DataLoss);
            }
            InworldBaseData inputData = inputs[0]; // YAN: Let's only process the last single input.
            InworldText textResult = new InworldText(inputData);
            if (textResult.IsValid)
                return ProcessTextData(textResult);

            InworldAudio audioResult = new InworldAudio(inputData);
            if (audioResult.IsValid)
                return ProcessAudioData(audioResult);
            
            return new InworldError($"Unsupported data type: {inputData.GetType()}", StatusCode.Unimplemented);
        }

        InworldBaseData ProcessTextData(InworldText txtData)
        {
            try
            {
                string text = txtData.Text;
                return new InworldText(text.ToUpper());
            }
            catch (Exception ex)
            {
                string strErr = $"[{NodeName}] Text processing error: {ex.Message}";
                Debug.LogError(strErr);
                return new InworldText(strErr);
            }
            
        }
        InworldBaseData ProcessAudioData(InworldAudio audioData)
        {
            try
            {
                InworldVector<float> waveform = audioData.Waveform;
                int sampleRate = audioData.SampleRate;

                InworldVector<float> processedWaveform = new InworldVector<float>();
                int wavSize = waveform.Size;
                for (int i = 0; i < wavSize; i++)
                {
                    float originalValue = waveform[i];
                    float processedValue = originalValue / 2.0f;
                    processedWaveform.Add(processedValue);
                }
                return new InworldAudio(processedWaveform, sampleRate);
            }
            catch (Exception ex)
            {
                string strErr = $"[{NodeName}] Audio processing error: {ex.Message}";
                Debug.LogError(strErr);
                return new InworldText(strErr);
            }
        }
    }
}