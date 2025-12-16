using Inworld.Framework.Graph;
using UnityEngine;

namespace Inworld.Framework.Samples.Node
{
    public class TextCombinerNodeAsset : CustomNodeAsset
    {
        public override string NodeTypeName => "TextCombinerNode";
        
        public string currentText = "";
        protected override InworldBaseData ProcessBaseData(InworldVector<InworldBaseData> inputs)
        {
            if (inputs.Size == 0)
            {
                return new InworldError("No input data", StatusCode.DataLoss);
            }
            InworldBaseData inputData = inputs[0];
            InworldText textResult = new InworldText(inputData);
            if (textResult.IsValid)
                currentText =  $"* {textResult.Text}";
            return new InworldText(currentText);
        }
        
        void OnEnable()
        {
            currentText = "";
        }
    }
}