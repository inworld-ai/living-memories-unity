#if UNITY_EDITOR
using Inworld.Framework.Graph;
using UnityEditor;

namespace Inworld.Framework.Editor
{
    [CustomEditor(typeof(CharacterInteractionGraphAsset))]
    public class CharacterInteractionGraphAssetEditor : InworldGraphAssetEditor
    {

    }
}
#endif