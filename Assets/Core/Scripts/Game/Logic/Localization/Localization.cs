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
        }

        public static Dictionary<string, string> LocalizationCode = new()
        {
            { "en", "English" },
            { "ru", "Russian" },
        };

        public static void OnСhangeLang(string lang)
        {
            Debug.Log("Correcting language");
            if (lang != "ru" && lang != "en")
            {
                YG2.lang = "en";
            }
            Debug.Log(YG2.lang);
            LocalizationManager.Language = YG2.lang switch
            {
                "ru" => "Russian",
                _ => "English"
            };
        }
    }
}