using System;
using System.Collections.Generic;
using System.Text;

namespace LeseEulenBibliothek.Core
{
    public static class TranslationService
    {
        private static string s_CurrentLanguage = "";
        private static Dictionary<string,string> s_Data = new Dictionary<string, string>();

        public static string CurrentLanguage
        {
            get => s_CurrentLanguage; 
            set
            {
                s_CurrentLanguage = value;
                UpdateLanguageData(value);
            }
        }

        private static void UpdateLanguageData(string value)
        {
            s_Data.Clear();
            var fileName = $"{value}.loc";
            if (!System.IO.File.Exists(fileName))
                return;
            var lines = System.IO.File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                var split = SplitKey(line);
                if (string.IsNullOrEmpty(split.key) || string.IsNullOrEmpty(split.value))
                    continue;
                s_Data[split.key] = split.value; 
            }
        }

        public static string GetTranslation(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            var split = SplitKey(key);
            if (string.IsNullOrEmpty(split.key))
                return "";
            if (s_Data.ContainsKey(split.key))
                return s_Data[split.key];
            return split.value;
        }

        private static (string key, string value) SplitKey(string key)
        {
            var split = key.Split("|");
            if(split.Length < 2)
                return (key, "");
                   
            return (split[0], split[1].Trim());
        }
    }
}
