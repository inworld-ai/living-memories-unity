using System;

namespace Inworld.Framework
{
    public class Language : InworldFrameworkDllClass
    {
        public Language()
        {
            m_DLLPtr = MemoryManager.Register(InworldInterop.inworld_Language_new(),
                InworldInterop.inworld_Language_delete);
        }
        
        public Language(IntPtr rhs)
        {
            m_DLLPtr = MemoryManager.Register(rhs, InworldInterop.inworld_Language_delete);
        }

        public string LanguageName
        {
            get => InworldInterop.inworld_Language_lang_get(m_DLLPtr);
            set => InworldInterop.inworld_Language_lang_set(m_DLLPtr, value);
        }

        public string Locale
        {
            get
            {
                IntPtr optStr = InworldInterop.inworld_Language_locale_get(m_DLLPtr);
                if (InworldInterop.inworld_optional_string_has_value(optStr))
                    return InworldInterop.inworld_optional_string_getConst(optStr);
                return null;
            }
            set
            {
                IntPtr optStr = InworldInterop.inworld_optional_string_new_rcstd_string(value);
                InworldInterop.inworld_Language_locale_set(m_DLLPtr, optStr);
            }
        }

        public static Language FromString(string languageName)
        {
            return new Language(InworldInterop.inworld_Language_FromString(languageName));
        }

        public override string ToString()
        {
            return InworldInterop.inworld_Language_ToString(m_DLLPtr);
        }
        
        public static Language English => new Language(InworldInterop.inworld_Language_English());

        public static Language Chinese => new Language(InworldInterop.inworld_Language_Chinese());

        public static Language Korean => new Language(InworldInterop.inworld_Language_Korean());

        public static Language Japanese => new Language(InworldInterop.inworld_Language_Japanese());

        public static Language Russian => new Language(InworldInterop.inworld_Language_Russian());

        public static Language Spanish => new Language(InworldInterop.inworld_Language_Spanish());
        
        public static Language French => new Language(InworldInterop.inworld_Language_French());
        
        public static Language German => new Language(InworldInterop.inworld_Language_German());

        public static Language Italian => new Language(InworldInterop.inworld_Language_Italian());
        
        public static Language Portuguese => new Language(InworldInterop.inworld_Language_Portuguese());
        
        public static Language Arabic => new Language(InworldInterop.inworld_Language_Arabic());

        public static Language Hindi => new Language(InworldInterop.inworld_Language_Hindi());
        
        public static Language Dutch => new Language(InworldInterop.inworld_Language_Dutch());
        
        public static Language Swedish => new Language(InworldInterop.inworld_Language_Swedish());
        
        public static Language Norwegian => new Language(InworldInterop.inworld_Language_Norwegian());

        public static Language Danish => new Language(InworldInterop.inworld_Language_Danish());

        public static Language Finnish => new Language(InworldInterop.inworld_Language_Finnish());

        public static Language Polish => new Language(InworldInterop.inworld_Language_Polish());

        public static Language Turkish => new Language(InworldInterop.inworld_Language_Turkish());

        public static Language Thai => new Language(InworldInterop.inworld_Language_Thai());

        public static Language Vietnamese => new Language(InworldInterop.inworld_Language_Vietnamese());

        public static Language Indonesian => new Language(InworldInterop.inworld_Language_Indonesian());

        public static Language Malay => new Language(InworldInterop.inworld_Language_Malay());

        public static Language Hebrew => new Language(InworldInterop.inworld_Language_Hebrew());
        
        public static Language Ukrainian => new Language(InworldInterop.inworld_Language_Ukrainian());
    }
}