using System;

namespace Inworld.Framework.Attributes
{
    public sealed class ModelTypeAttribute : Attribute
    {
        public ModelTypeAttribute(string forceEnumName)
        {
            ForceEnumName = forceEnumName;
        }
        public string ForceEnumName { get; }                 // "Remote", "LocalCPU" etc
        public bool LockAlways { get; set; }                 
        public string[] OnlyTargets { get; set; }            // "Android","iOS",etc
        public string[] ExcludeTargets { get; set; }         // "StandaloneWindows", "StandaloneWindows64", etc
    }
}
