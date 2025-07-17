using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using YG;

namespace Core
{
    public static class Localization
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            LocalizationManager.Read();
            YG2.onCorrectLang += OnСhangeLang;
            
            LocalizationManager.Language = YG2.lang switch
            {
                "ru" => "Russian",
                _ => "English"
            };
        }

        public static Dictionary<string, string> LocalizationCode = new()
        {
            { "en", "English" },
            { "ru", "Russian" },
        };

        private static void OnСhangeLang(string lang)
        {
            if (lang != "ru" && lang != "en")
            {
                YG2.lang = "en";
            }
        }
    }
}