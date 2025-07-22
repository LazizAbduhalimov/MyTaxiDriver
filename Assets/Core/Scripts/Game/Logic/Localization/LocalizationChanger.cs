using System;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using YG;

namespace Core
{
    public class LocalizationChanger : MonoBehaviour
    {
        private void OnEnable()
        {
            YG2.onSwitchLang += Localization.OnСhangeLang;
        }

        private void OnDisable()
        {
            YG2.onSwitchLang -= Localization.OnСhangeLang;
        }

        public void ChangeLanguage(string lang)
        {
            YG2.SwitchLanguage(lang);
        }
    }
}