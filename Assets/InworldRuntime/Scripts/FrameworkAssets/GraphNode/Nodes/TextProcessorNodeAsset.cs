/*************************************************************************************************
 * Copyright 2022-2025 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Framework.TextOperation;
using UnityEngine;

namespace Inworld.Framework.Graph
{
    /// <summary>
    /// Specialized node asset for text processing operations within graph workflows in the Inworld framework.
    /// Extends the custom node functionality to provide comprehensive text cleaning and filtering capabilities.
    /// For removing non-readable symbols, emojis, brackets, substrings, and other text processing operations.
    /// Input: InworldText or InworldDataStream&lt;string&gt;
    /// Output: InworldDataStream&lt;string&gt;
    /// This asset can be created through Unity's Create menu and used to sanitize and process text in conversation flows.
    /// </summary>
    // For Remove non-readable symbols and emojis.
    // Input: InworldText or InworldDataStream<string>
    // Output: InworldDataStream<string>
    [CreateAssetMenu(fileName = "Node_TextProcessor", menuName = "Inworld/Create Node/Text Operation/Text processor", order = -2498)]

    public class TextProcessorNodeAsset : CustomNodeAsset
    {
        public override string NodeTypeName => "TextProcessorNode";

        /// <summary>
        /// Whether to remove text enclosed in brackets from the input.
        /// When enabled, content within [], (), {} brackets will be filtered out.
        /// </summary>
        public bool needRemoveTextInBrackets = true;
        
        /// <summary>
        /// Whether to remove specified substrings from the input.
        /// When enabled, strings listed in substringsToRemove will be filtered out.
        /// </summary>
        public bool needRemoveSubstring = true;
        
        /// <summary>
        /// Whether to remove the player name from the input text.
        /// When enabled, the current player name will be added to the removal list.
        /// </summary>
        public bool needRemovePlayerName = true;
        
        /// <summary>
        /// Whether to process the input as sentence streams.
        /// When enabled, text will be broken down into individual sentences for processing.
        /// </summary>
        public bool needRemoveSentenceStream = true;
        
        /// <summary>
        /// Whether to remove emoji characters from the input text.
        /// When enabled, all emoji symbols will be filtered out from the text.
        /// </summary>
        public bool needRemoveEmojis = true;
        
        /// <summary>
        /// List of specific substrings to remove from the input text.
        /// Contains custom strings that should be filtered out during text processing.
        /// </summary>
        public List<string> substringsToRemove;
        
        /// <summary>
        /// Processes the input data through various text filtering and cleaning operations.
        /// Applies the configured text processing operations in sequence based on the enabled options.
        /// Handles text bracket removal, substring filtering, sentence streaming, and emoji removal.
        /// </summary>
        /// <param name="inputs">The vector of input data to process.</param>
        /// <returns>The processed text data as an InworldDataStream, or an error if processing fails.</returns>
        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            if (inputs.Size == 0)
            {
                return new InworldError("No input data", StatusCode.DataLoss);
            }
            InworldBaseData input = inputs[0];
            InworldDataStream<string> stream = new InworldDataStream<string>(input);
            if (!stream.IsValid)
                return new InworldError("Input Data is Invalid", StatusCode.FailedPrecondition);
            InworldInputStream<string> inputStream = stream.ToInputStream();
            if (needRemoveTextInBrackets)
            {
                TextInBracketsRemover textInBracketsRemover = new TextInBracketsRemover(inputStream);
                InworldInputStream<string> output = textInBracketsRemover.ToInputStream;
                return new InworldDataStream<string>(output, new CancellationContext(IntPtr.Zero)); // TODO(Yan): Figure out how to add Cancellation.
            }
            if (needRemoveSubstring)
            {
                if (needRemovePlayerName)
                    substringsToRemove.Add(InworldFrameworkUtil.PlayerName);
                InworldVector<string> vecSubStrs = new InworldVector<string>();
                vecSubStrs.FromList(substringsToRemove);
                SubstringRemover substringRemover = new SubstringRemover(inputStream, vecSubStrs);
                InworldInputStream<string> output = substringRemover.ToInputStream;
                return new InworldDataStream<string>(output, new CancellationContext(IntPtr.Zero)); // TODO(Yan): Figure out how to add Cancellation.
            }
            if (needRemoveSentenceStream)
            {
                SentenceStream sentenceStream = new SentenceStream(inputStream);
                InworldInputStream<string> output = sentenceStream.ToInputStream;
                return new InworldDataStream<string>(output, new CancellationContext(IntPtr.Zero)); // TODO(Yan): Figure out how to add Cancellation.
            }
            if (needRemoveEmojis)
            {
                EmojiRemover emojiRemover = new EmojiRemover(inputStream);
                InworldInputStream<string> output = emojiRemover.ToInputStream;
                return new InworldDataStream<string>(output, new CancellationContext(IntPtr.Zero)); // TODO(Yan): Figure out how to add Cancellation.
            }
            return inputs[0];
        }
    }
}