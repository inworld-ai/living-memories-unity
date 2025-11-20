using System;

namespace Inworld.Framework
{
    public class StringToStringBTreeMapStrategy : IBTreeMapStrategy<string, string>
    {
        public IntPtr CreateNew()
        {
            return InworldInterop.inworld_BTreeMap_StringToString_new();
        }

        public void Delete(IntPtr ptr)
        {
            InworldInterop.inworld_BTreeMap_StringToString_delete(ptr);
        }

        public int Size(IntPtr ptr)
        {
            return InworldInterop.inworld_BTreeMap_StringToString_size(ptr);
        }

        public bool IsEmpty(IntPtr ptr)
        {
            return InworldInterop.inworld_BTreeMap_StringToString_empty(ptr);
        }

        public void Clear(IntPtr ptr)
        {
            InworldInterop.inworld_BTreeMap_StringToString_clear(ptr);
        }

        public bool ContainsKey(IntPtr ptr, string key)
        {
            return InworldInterop.inworld_BTreeMap_StringToString___contains__(ptr, key);
        }

        public string GetValue(IntPtr ptr, string key)
        {
            return InworldInterop.inworld_BTreeMap_StringToString___getitem__(ptr, key);
        }

        public void SetValue(IntPtr ptr, string key, string value)
        {
            InworldInterop.inworld_BTreeMap_StringToString___setitem__(ptr, key, value);
        }
    }
}