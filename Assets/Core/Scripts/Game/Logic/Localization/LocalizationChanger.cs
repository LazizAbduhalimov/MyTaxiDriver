using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using YG;

namespace Core
{
    public class LocalizationChanger : MonoBehaviour
    {
        public void ChangeLanguage(string lang)
        {
            LocalizationManager.Language = Localization.LocalizationCode[lang];
            YG2.SwitchLanguage(lang);
        }
    }
}